using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Core.Doc
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
