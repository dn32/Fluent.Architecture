using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    public class FluentDocController : Controller
    {
        public static Dictionary<string, Type> Models { get; private set; }

        public FluentDocController()
        {
            if (Models == null)
            {
                Models = new Dictionary<string, Type>();
                Setup.Model.Values.ToList().ForEach(x =>
                {
                    Models.Add(x.Name, x);
                });
            }
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            var models = Setup.Model.Values.Select(x => x.GetFluentJsonSchema(false)).Where(x => x?.FluentJsonForm != null).ToList();
            return View(models);
        }

        // [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public IActionResult Service(string name)
        {
            if (Models.TryGetValue(name, out Type type))
            {
                var model = type.GetFluentJsonSchema(false);
                if (Setup.Controllers.TryGetValue(type, out Type controllerType))
                {
                    var routeAtributeController = controllerType.GetCustomAttribute<RouteAttribute>();
                    var actions = controllerType
                          .GetMethods()
                          .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                          .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                          .Select(action =>
                          {
                              var met = action.GetCustomAttribute<HttpMethodAttribute>();
                              var routeAtributeAction = action.GetCustomAttribute<RouteAttribute>();
                              var routerAttribute = routeAtributeAction?.Template ?? routeAtributeController?.Template;
                              var template = routerAttribute ?? met.Template ?? action.Name;

                              var route = template.Replace("[controller]", controllerType.Name.Remove("Controller"), StringComparison.InvariantCultureIgnoreCase);
                              route = route.Replace("[action]", action.Name, StringComparison.InvariantCultureIgnoreCase);
                              var name = route.Split("/").Last();
                              var method = met.HttpMethods.FirstOrDefault().Replace("DELETE", "DEL");

                              var orderMethod = method switch
                              {
                                  "GET" => 1,
                                  "POST" => 2,
                                  "PUT" => 3,
                                  "DEL" => 4,
                                  _ => 5,
                              };
                              
                              var parameters = action.GetParameters().Select(x => x.ParameterType);
                              var description = action.GetCustomAttribute<DescriptionAttribute>()?.Description;

                              return new FluentActionSchema
                              {
                                  ControllerType = controllerType,
                                  EntityType = type,
                                  Action = action,
                                  Name = name,
                                  Route = route,
                                  Method = method,
                                  OrderMethod = orderMethod,
                                  Parameters = parameters,
                                  Description = description
                              };
                          })
                          .ToList();

                    actions = actions.OrderBy(x => x.Name).OrderBy(x => x.OrderMethod).ToList();
                    ViewBag.actions = actions;
                }

                return View(model);
            }
            else
            {
                throw new InvalidOperationException($"Service {name} not found");
            }
        }
    }

    public static class stringExtension
    {
        public static string Remove(this string initialText, string removeText)
        {
            return initialText.Replace(removeText, "");
        }
    }
    public class FluentActionSchema
    {
        public Type EntityType { get; set; }
        public Type ControllerType { get; set; }
        public MethodInfo Action { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public int OrderMethod { get; internal set; }
        public IEnumerable<Type> Parameters { get; internal set; }
        public string Description { get; internal set; }
    }
}
