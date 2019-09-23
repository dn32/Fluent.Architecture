using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    internal class DocEmbeddedStaticFileProvider : IFileProvider
    {
        public EmbeddedFileProvider EmbeddedFileProvider { get; set; }

        public DocEmbeddedStaticFileProvider()
        {
            EmbeddedFileProvider = new EmbeddedFileProvider(typeof(FluentDocController).Assembly);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return EmbeddedFileProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (!subpath.StartsWith("FluentDoc"))
            {
                var info = GetFileInfo(subpath);
                return info;
            }

            var path = subpath.Replace("\\", "/");
            if (path.StartsWith("/")) { path = path.Substring(1, path.Length - 1); };
            path = Path.Combine("wwwroot", path);
            path = path.Replace("/", ".").Replace("\\", ".");
            return EmbeddedFileProvider.GetFileInfo(path);
        }

        public IChangeToken Watch(string filter)
        {
            return EmbeddedFileProvider.Watch(filter);
        }
    }

    public static class FluentDoc
    {
        public static void AddFluentDoc(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(opts =>
            {
                opts.FileProvider = new DocEmbeddedStaticFileProvider();
            });
        }
    }


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
                    Models.TryAdd(x.Name, x);
                });
            }
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            var models = Setup.Model.Values.Select(x => x.GetFluentJsonSchema(false)).Where(x => x?.FluentJsonForm != null).ToList();

            //var assembly = GetType().GetTypeInfo().Assembly;
            ////var resource = assembly.GetManifestResourceStream("MyLibrary._fonts.OpenSans.ttf");

            //var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            //var all = embeddedProvider.GetAllDataOfObject();
            //var lista = embeddedProvider.GetDirectoryContents("wwwroot.css.site.css");

            //using (var reader = embeddedProvider.GetFileInfo("index.html").CreateReadStream())
            //{
            //    // some logic with stream reader
            //}

            return View(models);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
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
}
