using Fluent.Architecture.Core.Services;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        [Route("FluentDoc/Authentication")]
        public IActionResult Index()
        {
            return View("/Views/FluentDoc/Authentication.cshtml");
        }

        [HttpPost]
        [Route("FluentDoc/Authentication")]
        public IActionResult Login(string user, string psw)
        {
            var service = ServiceFactory.Create(Setup.Config.Config.JwtInfo.FluentAuthenticationServiceType, HttpContext, "DocAuthenticationServiceType for FluentDoc").FluentCast<FluentAuthenticationService>();
            var token = service.Login(user, psw);
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new System.InvalidOperationException("Error trying to authenticate");
            }

            Response.Cookies.Append("Authorization", token, new CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
            return Redirect("/FluentDoc");
        }
    }
}
