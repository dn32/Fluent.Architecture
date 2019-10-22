using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Services;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Doc.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        [Route("FluentDoc/Authentication")]
        public IActionResult Index()
        {
            if (Setup.Config.Config.JwtInfo == null)
            {
                throw new InvalidOperationException("Use UseJwt at architecture startup to set authentication parameters");
            }

            return View("/Views/FluentDoc/Authentication.cshtml");
        }

        [HttpPost]
        [Route("FluentDoc/Authentication")]
        public async Task<IActionResult> LoginAsync(string email, string psw)
        {
            if(Setup.Config.Config.JwtInfo == null)
            {
                throw new InvalidOperationException("Use UseJwt at architecture startup to set authentication parameters");
            }

            var authenticationUser = new FluentAuthenticationUser
            {
                Email = email,
                Password = psw
            };

            var service = ServiceFactory.Create(Setup.Config.Config.JwtInfo.FluentAuthenticationServiceType, HttpContext, "DocAuthenticationServiceType for FluentDoc").FluentCast<FluentAuthenticationService>();
            var token = await service.LoginAsync(authenticationUser);
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Error trying to authenticate");
            }

            Response.Cookies.Append("Authorization", token, new CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
            return Redirect("/FluentDoc");
        }
    }
}
