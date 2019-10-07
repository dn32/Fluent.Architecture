using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    [AllowAnonymous]
    public class FluentDocController : Controller
    {
        public static Dictionary<string, Type> Models { get; private set; }

        public FluentDocController()
        {
            if (Models == null)
            {
                Models = new Dictionary<string, Type>();
                var entities = Setup.GetFluentApiEntity();
                entities.ForEach(x =>
                {
                    Models.TryAdd(x.Name, x);
                });
            }
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        [Route("FluentDoc")]
        [Route("FluentDoc/Index")]
        public IActionResult Index()
        {
            var models = Models.Values.Select(x => x.GetFluentJsonSchema(false)).ToList();
            return View(models);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        [Route("FluentDoc/Service")]
        public IActionResult Service(string name)
        {
            if (Models.TryGetValue<string, Type>(name, StringComparison.InvariantCultureIgnoreCase, out Type type))
            {
                if (Setup.Controllers.TryGetValue(type, out Type controllerType))
                {
                    var model = type.GetFluentJsonSchema(false);
                    if (model != null)
                    {
                        var routeAtributeController = controllerType.GetCustomAttributes<RouteAttribute>(true).FirstOrDefault();
                        var actions = controllerType
                              .GetMethods()
                              .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                              .Where(method => !method.Name.StartsWith("get_") && !method.Name.Equals("Dispose") && !method.Name.Equals("GetType") && !method.Name.StartsWith("set_"))
                              .Select(action =>
                              {
                                  var met = action.GetCustomAttribute<HttpMethodAttribute>() ?? new HttpGetAttribute();
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

                                  var parameters = action.GetParameters().Select(x => new DocParameter { Type = x.ParameterType, Name = x.Name });
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
                                      Description = description,
                                      ApiBaseUrl = FluentDocExtension.ApiBaseUrl
                                  };
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
    }
}
