using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Doc.Attributes;
using Fluent.Architecture.Core.Enumerator;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    [AllowAnonymous]
    public class FluentDocController : Controller
    {
        public static Dictionary<string, Type> Models { get; private set; }
        public static Dictionary<string, Type> AllTypes { get; private set; }
        private static object initialLock { get; set; } = new object();

        public FluentDocController()
        {
            lock (initialLock)
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
                    AllTypes = AppDomain.CurrentDomain
                              .GetAssemblies()
                              .Where(x => !x.IsDynamic)
                              .SelectMany(x => x.GetTypes())
                              .ToList()
                              .GroupBy(x => x.FullName)
                              .Select(x => x.First())
                              .ToDictionary(x => x.FullName, x => x);
                }
            }
        }

        [Route("FluentDoc"), Route("FluentDoc/Index")]
        public IActionResult Index()
        {
            var models = Models.Values
                .Where(x => x.GetCustomAttribute<FluentDocAttribute>()?.Display != EnumFluentDisplay.Hidden)
                .Where(x => x.GetCustomAttribute<FluentAPIControllerAttribute>()?.AutomaticGeneration != false)
                .Select(x => x.GetFluentJsonSchema(false)).ToList();

            return View(models);
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
        //[ResponseCache(Duration = 60000, Location = ResponseCacheLocation.Client)]
        public IActionResult Model(string name)
        {
            if (AllTypes.TryGetValue(name, out Type type))
            {
                var jsonSchema = type.GetFluentJsonSchema(false);
                return View(jsonSchema);
            }

            return Content("Model not found");
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

        public static string GetModelLink(string modelName)
        {
            if (AllTypes.TryGetValue(modelName, out _)) { return $"/FluentDoc/Model?name={modelName}"; }
            return modelName;
        }

        public static string GetModelLink(Type type)
        {
            return $"/FluentDoc/Model?name={type.GetListTypeNonNull().FullName}";
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

            var parameters = action.GetParameters().Select(x => new DocParameter { Link = GetModelLink(x.ParameterType), Type = x.ParameterType, Name = x.Name, Source = GetParameterSource(x, orderMethod) }).ToList();
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
                parameters.Add(new DocParameter("Authorization", typeof(string), EnumParameterSouce.Header, "Authentication Token", "Bearer xxxxx"));
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

        private string GetExampleAction(FluentActionSchema action)
        {
            var parametersArray = action.Parameters.Where(x => x.Source == EnumParameterSouce.Header).Select(x => $"xhr.setRequestHeader(\"{x.Name}\", \"{x.Example}\";").ToArray();
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
xhr.open(""{action.Method}"", ""{action.ApiBaseUrl}{action.Route}"");
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
    }
}
