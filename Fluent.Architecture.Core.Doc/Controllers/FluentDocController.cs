using dn32.infra.extensoes;
using dn32.infra.nucleo.atributos;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Nucleo.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using dn32.infra.atributos;
using dn32.infra.enumeradores;

namespace dn32.infra.Nucleo.Doc.Controllers
{

#if (!DEBUG)
    [ResponseCache(Duration = 60000, Location = ResponseCacheLocation.Client)]
#endif
    [AllowAnonymous]
    public partial class DnDocController : Controller
    {
        #region PROPERTIES

        private static Dictionary<string, Type> Models { get; set; }
        private static Dictionary<string, Type> AllTypes { get; set; }
        private static List<EntityModelAndName> AllEntities { get; set; }
        private static List<EntityModelAndName> AllModel { get; set; }
        private static object InitialLock { get; set; } = new object();

        #endregion

        public DnDocController()
        {
            Initialize();
        }

        [Route("DnDoc"), Route("DnDoc/Index")]
        public IActionResult Index()
        {
            return View(AllEntities);
        }

        [Route("DnDoc/Action")]
        public IActionResult Action(string service, string actionName)
        {
            if (Models.TryGetValue<string, Type>(service, StringComparison.InvariantCultureIgnoreCase, out Type type))
            {
                if (Setup.Controladores.TryGetValue(type, out Type controllerType))
                {
                    if (controllerType.GetCustomAttribute<DnDocAtributo>()?.Apresentacao == EnumApresentar.Ocultar)
                    {
                        throw new InvalidOperationException("DnDocAtributoAttribute is EnumDnMostrar.Hidden");
                    }

                    var model = type.GetDnJsonSchema(false);
                    if (model != null)
                    {
                        var routeAtributeController = controllerType.GetCustomAttributes<RouteAttribute>(true).FirstOrDefault();
                        var actionMethod = controllerType
                              .GetMethods()
                              .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                              .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                              .Where(method => method.GetCustomAttribute<DnDocAtributo>()?.Apresentacao != EnumApresentar.Ocultar)
                              .FirstOrDefault(method => method.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase));

                        if (actionMethod == null) { return Content("Action not found"); }

                        var actionData = GetActionData(actionMethod, type, controllerType, routeAtributeController).First();

                        return View(actionData);
                    }
                    else
                    {
                        return Content("JsonSchema not found");
                    }
                }
                else
                {
                    return Content("Controller not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Servico {service} not found");
            }
        }

        [Route("DnDoc/Model")]
        public IActionResult Model(string name)
        {
            if (AllTypes.TryGetValue(name, out Type type))
            {
                var jsonSchema = type.GetDnJsonSchema(false);
                jsonSchema.Formulario.Nome = type.GetFriendlyName();
                jsonSchema.Propriedades.Where(x => x.Propriedade.GetCustomAttribute<DnDocAtributo>()?.Apresentacao != EnumApresentar.Ocultar).ToList()
                .ForEach(x =>
                {
                    x.Descricao = x.Descricao.G();
                    x.Link = GetModelLink(x.Tipo);
                });

                if (type.IsNullableEnum())
                {
                    jsonSchema.Propriedades = type.GetFields().Where(x => x.Name != "value__").Select(x =>
                    new DnPropriedadeJsonAtributo
                    {
                        NomeDaPropriedade = x.Name,
                        Nome = (x.GetCustomAttribute<DnPropriedadeJsonAtributo>(true)?.Descricao ?? x.Name.ToLower().ToTitleCase()),
                        Descricao = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<DnPropriedadeJsonAtributo>(true)?.Descricao ?? x.Name).G(),
                        Formulario = EnumTipoDeComponenteDeFormularioDeTela.Texto,
                        Tipo = x.FieldType.BaseType
                    }).ToList();
                }

                return View(jsonSchema);
            }

            return Content("Model not found");
        }

        [Route("DnDoc/Entidade")]
        public IActionResult Entity()
        {
            return View(AllEntities);
        }

        [Route("DnDoc/ModelNoEntity")]
        public IActionResult ModelNoEntity()
        {
            return View(AllModel);
        }

        [Route("DnDoc/Servico")]
        public IActionResult Service(string name)
        {
            if (Models.TryGetValue<string, Type>(name, StringComparison.InvariantCultureIgnoreCase, out Type type))
            {
                if (Setup.Controladores.TryGetValue(type, out Type controllerType))
                {
                    if (controllerType.GetCustomAttribute<DnDocAtributo>()?.Apresentacao == EnumApresentar.Ocultar)
                    {
                        throw new InvalidOperationException("DnDocAtributoAttribute is EnumDnMostrar.Hidden");
                    }

                    var model = type.GetDnJsonSchema(false);
                    if (model != null)
                    {
                        var routeAtributeController = controllerType.GetCustomAttributes<RouteAttribute>(true).FirstOrDefault();
                        var actions = controllerType
                              .GetMethods()
                              .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                              .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                              .Where(method => method.GetCustomAttribute<DnDocAtributo>()?.Apresentacao != EnumApresentar.Ocultar)
                              .SelectMany(action =>
                              {
                                  return GetActionData(action, type, controllerType, routeAtributeController);
                              })
                              .ToList();

                        actions = actions.OrderBy(x => x.Name).OrderBy(x => x.OrderMethod).ToList();
                        ViewBag.actions = actions;
                        return View(model);
                    }
                    else
                    {
                        return Content("JsonSchema not found");
                    }
                }
                else
                {
                    return Content("Controller not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Servico {name} not found");
            }
        }

        #region PRIVATE

        private string GetReturn(MethodInfo methodInfo)
        {
            var returnType = methodInfo.ReturnType.GetTaskType();
            return returnType.GetFriendlyName(false, true);
        }

        private EnumParameterSouce GetParameterSource(ParameterInfo parameterInfo, int orderMethod)
        {
            if (parameterInfo.IsDefined(typeof(FromQueryAttribute))) { return EnumParameterSouce.Query; }
            if (parameterInfo.IsDefined(typeof(FromBodyAttribute))) { return EnumParameterSouce.Body; }
            if (parameterInfo.IsDefined(typeof(FromFormAttribute))) { return EnumParameterSouce.Form; }
            if (parameterInfo.IsDefined(typeof(FromRouteAttribute))) { return EnumParameterSouce.Route; }
            if (parameterInfo.IsDefined(typeof(FromHeaderAttribute))) { return EnumParameterSouce.Header; }
            if (parameterInfo.IsDefined(typeof(FromServicesAttribute))) { return EnumParameterSouce.Service; }
            if (orderMethod == 2 || orderMethod == 3) { return EnumParameterSouce.Body; } else return EnumParameterSouce.Query;
        }

        internal static string GetModelLink(Type type)
        {
            var fullName = type.GetListTypeNonNull().FullName;
            if (string.IsNullOrWhiteSpace(fullName)) { return string.Empty; }
            if (AllTypes.TryGetValue(type.GetListTypeNonNull().FullName, out _)) { return $"/DnDoc/Model?Nome={type.GetListTypeNonNull().FullName}"; }
            return string.Empty;
        }

        private DnActionSchema[] GetActionData(MethodInfo action, Type type, Type controllerType, RouteAttribute routeAtributeController)
        {
            var mets = action.GetCustomAttributes<HttpMethodAttribute>() ?? new List<HttpGetAttribute> { new HttpGetAttribute() };
            return mets.Select(x => Onter(x)).ToArray();

            DnActionSchema Onter(HttpMethodAttribute met)
            {
                var routeAtributeAction = action.GetCustomAttribute<RouteAttribute>();
                var routerAttribute = routeAtributeAction?.Template ?? routeAtributeController?.Template;
                var template = routerAttribute ?? met.Template ?? action.Name;

                var route = template.Replace("[controller]", controllerType.Name.Remove("Controller"), StringComparison.InvariantCultureIgnoreCase);
                route = route.Replace("[action]", action.Name, StringComparison.InvariantCultureIgnoreCase);
                var name = route.Split("/").Last();
                var method = met.HttpMethods.FirstOrDefault().Replace("DELETE", "DEL");
                var methodName = action.Name;

                var returnType = GetReturn(action);

                var orderMethod = method switch
                {
                    "GET" => 1,
                    "POST" => 2,
                    "PUT" => 3,
                    "DEL" => 4,
                    _ => 5,
                };

                var parameters = action.GetParameters().Select(x =>
                                new DocParameter
                                {
                                    Link = GetModelLink(x.ParameterType),
                                    Type = x.ParameterType,
                                    Name = x.Name,
                                    Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<DnPropriedadeJsonAtributo>(true)?.Descricao ?? x.Name).G(),
                                    Source = GetParameterSource(x, orderMethod),
                                    Example = x.ParameterType.GetExampleValueString()
                                }).ToList();

                var description = action.GetCustomAttribute<DescriptionAttribute>()?.Description;
                var fluentAction = action.GetCustomAttribute<DnActionAtributo>();

                if (fluentAction?.Paginacao == true)
                {
                    parameters.AddRange(new[] {
                    new DocParameter("CurrentPage", typeof(string), EnumParameterSouce.Header, "The current page", "1"),
                    new DocParameter("ItemsPerPage", typeof(string), EnumParameterSouce.Header, "The number of items per page", "10"),
                    new DocParameter("StartAtZero", typeof(bool), EnumParameterSouce.Header, "If the first page is 0", "true")
                });
                }

                if (fluentAction?.EspecificacaoDinamica == true)
                {
                    parameters.AddRange(new[] {
                    //new DocParameter("PropertyToIgnore", typeof(string), EnumParameterSouce.Header, "The properties you want to ignore in the query", "Code,Adress.Code"),
                    new DocParameter("PropertyToShow", typeof(string), EnumParameterSouce.Header, "The properties you want to get in the query", "LastName,FirstName,Code,Andress.Name".G()),
                    new DocParameter("PropertyToOrder", typeof(string), EnumParameterSouce.Header, "The properties by which to sort", "LastName,FirstName".G())
                });
                }


                if (Setup.ConfiguracoesGlobais.InformacoesDoJWT != null)
                {
                    parameters.Add(new DocParameter("Authorization", typeof(string), EnumParameterSouce.Header, "The authentication Token", "Bearer xxxxx"));
                }

                var action_ = new DnActionSchema
                {
                    ControllerType = controllerType,
                    EntityType = type,
                    Action = action,
                    Name = name,
                    Route = route,
                    Method = method,
                    OrderMethod = orderMethod,
                    Parameters = parameters,
                    Description = description.G(),
                    ApiBaseUrl = DnDocExtension.ApiBaseUrl,
                    ReturnType = returnType,
                    MethodName = methodName
                };

                action_.Example = GetExampleAction(action_);

                return action_;
            }
        }

        private List<string> JsonToQueryString(string json)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            if (jObj == null) { return default; }
            return jObj.Children().Cast<JProperty>().Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())).ToList();
        }

        private string GetExampleAction(DnActionSchema action)
        {
            var parametersArray = action.Parameters.Where(x => x.Source == EnumParameterSouce.Header).Select(x => $"xhr.setRequestHeader(\"{x.Name}\", \"{x.Example}\");").ToArray();
            var parametersQueryArray = action.Parameters.Where(x => x.Source == EnumParameterSouce.Query).Where(x => !x.Type.IsDnEntity()).Select(x => $"{x.Name}={x.Example}").ToList();
            var parametersQueryArray3 = action.Parameters.Where(x => x.Source == EnumParameterSouce.Query).Where(x => x.Type.IsDnEntity()).SelectMany(x => JsonToQueryString(x.Example)).ToList();
            parametersQueryArray.AddRange(parametersQueryArray3);

            var parametersQueryArrayString = "";

            if (parametersQueryArray.Count > 0)
            {
                parametersQueryArrayString = "?" + string.Join("&", parametersQueryArray);
            }

            var parametersString = string.Join('\n', parametersArray);
            var dataExample = "xhr.send();";

            if (action.Method == "POST" || action.Method == "PUT")
            {
                dataExample =
    $@"var data = JSON.stringify({{
    ""test"": ""a""
}});

xhr.send(data);";
            }

            var example =
    $@"var xhr = new XMLHttpRequest();
xhr.withCredentials = true;
xhr.open(""{action.Method}"", ""{action.ApiBaseUrl}{action.Route}{parametersQueryArrayString}"");
{parametersString}

xhr.addEventListener(""readystatechange"", function() {{
    if (this.readyState === 4)
    {{
        console.log(this.responseText);
    }}
}});

{dataExample}
";
            return example;
        }

        private void Initialize()
        {
            lock (InitialLock)
            {
                if (Models == null)
                {
                    Models = new Dictionary<string, Type>();
                    var entities = Setup.ObterEntidades();
                    entities.ForEach(type =>
                    {

                        if (Setup.Controladores.TryGetValue(type, out Type controllerType))
                        {
                            if (controllerType.GetCustomAttribute<DnDocAtributo>()?.Apresentacao == EnumApresentar.Ocultar)
                            {
                                return;
                            }
                        }

                        Models.TryAdd(type.Name, type);
                    });
                }

                if (AllTypes == null)
                {
                    AllTypes = Setup.TodosOsTipos
                                  .GroupBy(x => x.FullName)
                                  .Select(x => x.First())
                                  .Where(x => x.GetCustomAttribute<DnDocAtributo>()?.Apresentacao != EnumApresentar.Ocultar)
                                  .Where(x => x.GetCustomAttribute<DnControladorApiAtributo>()?.GerarAutomaticamente != false)
                                  .OrderBy(x => x.Name)
                                  .ToDictionary(x => x.FullName, x => x);
                }

                AllEntities = Models.Values
                    .Where(x => x.GetCustomAttribute<DnDocAtributo>()?.Apresentacao != EnumApresentar.Ocultar)
                    .Where(x => x.GetCustomAttribute<DnControladorApiAtributo>()?.GerarAutomaticamente != false)
                    .Select(x => new EntityModelAndName
                    {
                        Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<DnFormularioJsonAtributo>(true)?.Descricao ?? x.Name).G(),
                        FriendlyName = x.GetCustomAttribute<DnFormularioJsonAtributo>(true)?.Nome ?? x.GetFriendlyName().ToLower().ToTitleCase(),
                        Name = x.Name.ToDnJsonStringNormalized(),
                        FullName = x.FullName
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                AllModel = AllTypes.Values
                    .Where(x => x != null)
                    .Where(x => !Setup.Modelos.ContainsKey(x))
                    .Where(x => !Setup.Servicos.ContainsKey(x))
                    .Where(x => !Setup.Repositorios.ContainsKey(x))
                    .Where(x => !Setup.Controladores.ContainsKey(x))
                    .Where(x => !Setup.Validacoes.ContainsKey(x))
                    .Where(x => !x.Is(typeof(Controller)))
                    .Where(x => !x.Is(typeof(ControllerBase)))
                    .Where(x => x.FullName?.Contains("+") == false)
                    .Where(x => x.FullName?.StartsWith("System") == false)
                    .Where(x => x.FullName?.StartsWith("Windows") == false)
                    .Where(x => x.FullName?.StartsWith("Microsoft") == false)
                    .Where(x => x.FullName?.StartsWith("Internal") == false)
                    .Where(x => x.FullName?.StartsWith("FxResources") == false)
                    .Where(x => x.GetCustomAttribute<DnDocAtributo>()?.Apresentacao == EnumApresentar.Mostrar)
                    .Select(x => new EntityModelAndName
                    {
                        Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<DnFormularioJsonAtributo>(true)?.Descricao ?? x.GetFriendlyName()).G(),
                        FriendlyName = x.GetCustomAttribute<DnFormularioJsonAtributo>(true)?.Nome ?? x.GetFriendlyName(),
                        Name = x.Name.ToDnJsonStringNormalized(),
                        FullName = x.FullName
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }

        #endregion
    }
}
