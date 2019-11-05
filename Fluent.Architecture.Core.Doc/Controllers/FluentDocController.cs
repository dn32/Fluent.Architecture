using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Extensions;
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
using System.Threading.Tasks;
using System.Web;

namespace Fluent.Architecture.Core.Doc.Controllers
{

#if (!DEBUG)
    [ResponseCache(Duration = 60000, Location = ResponseCacheLocation.Client)]
#endif
    [AllowAnonymous]
    public partial class FluentDocController : Controller
    {
        #region PROPERTIES

        private static Dictionary<string, Type> Models { get; set; }
        private static Dictionary<string, Type> AllTypes { get; set; }
        private static List<EntityModelAndName> AllEntities { get; set; }
        private static List<EntityModelAndName> AllModel { get; set; }
        private static object InitialLock { get; set; } = new object();
        
        #endregion

        public FluentDocController()
        {
            Initialize();
        }
        
        [Route("FluentDoc"), Route("FluentDoc/Index")]
        public IActionResult Index()
        {
            return View(AllEntities);
        }

        [Route("FluentDoc/Action")]
        public IActionResult Action(string service, string actionName)
        {
            if (Models.TryGetValue<string, Type>(service, StringComparison.InvariantCultureIgnoreCase, out Type type))
            {
                if (Setup.Controllers.TryGetValue(type, out Type controllerType))
                {
                    if (controllerType.GetCustomAttribute<FluentDocAttribute>()?.Display == EnumFluentDisplay.Hidden)
                    {
                        throw new InvalidOperationException("FluentDocAttributeAttribute is EnumFluentDisplay.Hidden");
                    }

                    var model = type.GetFluentJsonSchema(false);
                    if (model != null)
                    {
                        var routeAtributeController = controllerType.GetCustomAttributes<RouteAttribute>(true).FirstOrDefault();
                        var actionMethod = controllerType
                              .GetMethods()
                              .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                              .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                              .Where(method => method.GetCustomAttribute<FluentDocAttribute>()?.Display != EnumFluentDisplay.Hidden)
                              .FirstOrDefault(method => method.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase));

                        if (actionMethod == null) { return Content("Action not found"); }

                        var actionData = GetActionData(actionMethod, type, controllerType, routeAtributeController);

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
                throw new InvalidOperationException($"Service {service} not found");
            }
        }

        [Route("FluentDoc/Model")]
        public IActionResult Model(string name)
        {
            if (AllTypes.TryGetValue(name, out Type type))
            {
                var jsonSchema = type.GetFluentJsonSchema(false);
                jsonSchema.FluentJsonForm.name = type.GetFriendlyName();

                jsonSchema.Properties.ForEach(x =>
                {
                    x.desc = x.desc.G();
                    x.Link = GetModelLink(x.Type);
                });

                if (type.IsNullableEnum())
                {
                    jsonSchema.Properties = type.GetFields().Where(x => x.Name != "value__").Select(x =>
                    new FluentJsonPropertyAttribute
                    {
                        propName = x.Name,
                        name = (x.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.desc ?? x.Name.ToLower().ToTitleCase()),
                        desc = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.desc ?? x.Name).G(),
                        form = EnumForm.TEXTBOX,
                        Type = x.FieldType.BaseType
                    }).ToList();
                }

                return View(jsonSchema);
            }

            return Content("Model not found");
        }

        [Route("FluentDoc/Entity")]
        public IActionResult Entity()
        {
            return View(AllEntities);
        }

        [Route("FluentDoc/ModelNoEntity")]
        public IActionResult ModelNoEntity()
        {
            return View(AllModel);
        }

        [Route("FluentDoc/Service")]
        public IActionResult Service(string name)
        {
            if (Models.TryGetValue<string, Type>(name, StringComparison.InvariantCultureIgnoreCase, out Type type))
            {
                if (Setup.Controllers.TryGetValue(type, out Type controllerType))
                {
                    if (controllerType.GetCustomAttribute<FluentDocAttribute>()?.Display == EnumFluentDisplay.Hidden)
                    {
                        throw new InvalidOperationException("FluentDocAttributeAttribute is EnumFluentDisplay.Hidden");
                    }

                    var model = type.GetFluentJsonSchema(false);
                    if (model != null)
                    {
                        var routeAtributeController = controllerType.GetCustomAttributes<RouteAttribute>(true).FirstOrDefault();
                        var actions = controllerType
                              .GetMethods()
                              .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                              .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                              .Where(method => method.GetCustomAttribute<FluentDocAttribute>()?.Display != EnumFluentDisplay.Hidden)
                              .Select(action =>
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
                throw new InvalidOperationException($"Service {name} not found");
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
            if (parameterInfo.GetCustomAttributeAny<FromQueryAttribute>()) { return EnumParameterSouce.Query; }
            if (parameterInfo.GetCustomAttributeAny<FromBodyAttribute>()) { return EnumParameterSouce.Body; }
            if (parameterInfo.GetCustomAttributeAny<FromFormAttribute>()) { return EnumParameterSouce.Form; }
            if (parameterInfo.GetCustomAttributeAny<FromRouteAttribute>()) { return EnumParameterSouce.Route; }
            if (parameterInfo.GetCustomAttributeAny<FromHeaderAttribute>()) { return EnumParameterSouce.Header; }
            if (parameterInfo.GetCustomAttributeAny<FromServicesAttribute>()) { return EnumParameterSouce.Service; }
            if (orderMethod == 2 || orderMethod == 3) { return EnumParameterSouce.Body; } else return EnumParameterSouce.Query;
        }
      
        internal static string GetModelLink(Type type)
        {
            var fullName = type.GetListTypeNonNull().FullName;
            if (string.IsNullOrWhiteSpace(fullName)) { return string.Empty; }
            if (AllTypes.TryGetValue(type.GetListTypeNonNull().FullName, out _)) { return $"/FluentDoc/Model?name={type.GetListTypeNonNull().FullName}"; }
            return string.Empty;
        }

        private FluentActionSchema GetActionData(MethodInfo action, Type type, Type controllerType, RouteAttribute routeAtributeController)
        {
            var met = action.GetCustomAttribute<HttpMethodAttribute>() ?? new HttpGetAttribute();
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
                                Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.desc ?? x.Name).G(),
                                Source = GetParameterSource(x, orderMethod),
                                Example = x.ParameterType.GetExampleValueString()
                            }).ToList();

            var description = action.GetCustomAttribute<DescriptionAttribute>()?.Description;
            var fluentAction = action.GetCustomAttribute<FluentActionAttribute>();

            if (fluentAction?.Pagination == true)
            {
                parameters.AddRange(new[] {
                    new DocParameter("CurrentPage", typeof(string), EnumParameterSouce.Header, "The current page", "1"),
                    new DocParameter("ItemsPerPage", typeof(string), EnumParameterSouce.Header, "The number of items per page", "10"),
                    new DocParameter("StartAtZero", typeof(bool), EnumParameterSouce.Header, "If the first page is 0", "true")
                });
            }

            if (fluentAction?.DynamicSpec == true)
            {
                parameters.AddRange(new[] {
                    //new DocParameter("PropertyToIgnore", typeof(string), EnumParameterSouce.Header, "The properties you want to ignore in the query", "Code,Adress.Code"),
                    new DocParameter("PropertyToShow", typeof(string), EnumParameterSouce.Header, "The properties you want to get in the query", "LastName,FirstName,Code,Andress.Name".G()),
                    new DocParameter("PropertyToOrder", typeof(string), EnumParameterSouce.Header, "The properties by which to sort", "LastName,FirstName".G())
                });
            }


            if (Setup.Config.Config.JwtInfo != null)
            {
                parameters.Add(new DocParameter("Authorization", typeof(string), EnumParameterSouce.Header, "The authentication Token", "Bearer xxxxx"));
            }

            var action_ = new FluentActionSchema
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
                ApiBaseUrl = FluentDocExtension.ApiBaseUrl,
                ReturnType = returnType,
                MethodName = methodName
            };

            action_.Example = GetExampleAction(action_);

            return action_;
        }

        private List<string> JsonToQueryString(string json)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            return jObj.Children().Cast<JProperty>().Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())).ToList();
        }

        private string GetExampleAction(FluentActionSchema action)
        {
            var parametersArray = action.Parameters.Where(x => x.Source == EnumParameterSouce.Header).Select(x => $"xhr.setRequestHeader(\"{x.Name}\", \"{x.Example}\");").ToArray();
            var parametersQueryArray = action.Parameters.Where(x => x.Source == EnumParameterSouce.Query).Where(x => !x.Type.IsFluentEntity()).Select(x => $"{x.Name}={x.Example}").ToList();
            var parametersQueryArray3 = action.Parameters.Where(x => x.Source == EnumParameterSouce.Query).Where(x => x.Type.IsFluentEntity()).SelectMany(x => JsonToQueryString(x.Example)).ToList();
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
                    var entities = Setup.GetFluentApiEntity();
                    entities.ForEach(type =>
                    {

                        if (Setup.Controllers.TryGetValue(type, out Type controllerType))
                        {
                            if (controllerType.GetCustomAttribute<FluentDocAttribute>()?.Display == EnumFluentDisplay.Hidden)
                            {
                                return;
                            }
                        }

                        Models.TryAdd(type.Name, type);
                    });
                }

                if (AllTypes == null)
                {
                    AllTypes = Setup.AllTypes
                                  .GroupBy(x => x.FullName)
                                  .Select(x => x.First())
                                  .Where(x => x.GetCustomAttribute<FluentDocAttribute>()?.Display != EnumFluentDisplay.Hidden)
                                  .Where(x => x.GetCustomAttribute<FluentAPIControllerAttribute>()?.AutomaticGeneration != false)
                                  .ToDictionary(x => x.FullName, x => x);
                }

                AllEntities = Models.Values
                    .Where(x => x.GetCustomAttribute<FluentDocAttribute>()?.Display != EnumFluentDisplay.Hidden)
                    .Where(x => x.GetCustomAttribute<FluentAPIControllerAttribute>()?.AutomaticGeneration != false)
                    .Select(x => new EntityModelAndName
                    {
                        Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<FluentJsonFormAttribute>(true)?.desc ?? x.Name).G(),
                        FriendlyName = x.GetCustomAttribute<FluentJsonFormAttribute>(true)?.name ?? x.GetFriendlyName().ToLower().ToTitleCase(),
                        Name = x.Name.ToFluentJsonStringNormalized(),
                        FullName = x.FullName
                    }).ToList();

                AllModel = AllTypes.Values
                    .Where(x => !Setup.Model.ContainsKey(x))
                    .Where(x => !Setup.Services.ContainsKey(x))
                    .Where(x => !Setup.Repositories.ContainsKey(x))
                    .Where(x => !Setup.Controllers.ContainsKey(x))
                    .Where(x => !Setup.Validations.ContainsKey(x))
                    .Where(x => !x.Is(typeof(Controller)))
                    .Where(x => !x.Is(typeof(ControllerBase)))
                    .Where(x => !x.FullName.Contains("+"))
                    .Where(x => !x.FullName.StartsWith("System"))
                    .Where(x => !x.FullName.StartsWith("Windows"))
                    .Where(x => !x.FullName.StartsWith("Microsoft"))
                    .Where(x => !x.FullName.StartsWith("Internal"))
                    .Where(x => !x.FullName.StartsWith("FxResources"))
                    .Where(x => x.GetCustomAttribute<FluentDocAttribute>()?.Display == EnumFluentDisplay.Show)
                    .Select(x => new EntityModelAndName
                    {
                        Description = (x.GetCustomAttribute<DescriptionAttribute>(true)?.Description ?? x.GetCustomAttribute<FluentJsonFormAttribute>(true)?.desc ?? x.GetFriendlyName()).G(),
                        FriendlyName = x.GetCustomAttribute<FluentJsonFormAttribute>(true)?.name ?? x.GetFriendlyName(),
                        Name = x.Name.ToFluentJsonStringNormalized(),
                        FullName = x.FullName
                    }).ToList();
            }
        }

        #endregion
    }
}
