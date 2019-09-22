using Fluent.Architecture.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public IActionResult Service(string name)
        {
            if (Models.TryGetValue(name, out Type type))
            {
                var model = type.GetFluentJsonSchema(false);
                return View(model);
            }
            else
            {
                throw new InvalidOperationException($"Service {name} not found");
            }
        }
    }
}
