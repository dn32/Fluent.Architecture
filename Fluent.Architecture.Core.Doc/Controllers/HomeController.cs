using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fluent.Architecture.Core.Doc.Models;
using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            if (Models == null)
            {
                Models = new Dictionary<string, Type>();
                Setup.Model.Values.ToList().ForEach(x =>
                {
                    Models.Add(x.Name, x);
                });
            }
        }

        public IActionResult Index()
        {
            var models = Setup.Model.Values.Select(x => x.GetFluentJsonSchema(false)).Where(x => x?.FluentJsonForm != null).ToList();
            return View(models);
        }

        public static Dictionary<string, Type> Models { get; private set; }

        public IActionResult Service(string name)
        {
           if(Models.TryGetValue(name, out Type type))
            {
                var model = type.GetFluentJsonSchema(false);
                return View(model);

            }
            else
            {
                throw new InvalidOperationException($"Service {name} not found");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
