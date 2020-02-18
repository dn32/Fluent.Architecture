using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Nucleo.Services;
using Fluente.Arquitetura.Base.Models;
using Fluente.Arquitetura.Base.Atributos;
using Fluente.Arquitetura.Base.Enumeradores;
using Fluente.Arquitetura.Base.Extensoes;
using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fluente.Arquitetura.Nucleo.Doc.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        [Route("FluenteDoc/Authentication")]
        public IActionResult Index()
        {
            if (Setup.Config.Config.JwtInfo == null)
            {
                throw new InvalidOperationException("Use UseJwt at Arquitetura startup to set authentication parameters");
            }

            return View("/Views/FluenteDoc/Authentication.cshtml");
        }

        [HttpGet]
        [Route("FluenteDoc/Token")]
        public IActionResult Token()
        {
            var token = Request.Cookies["Authorization"];
            if (string.IsNullOrWhiteSpace(token)) { return RedirectToAction(nameof(Index)); }
            ViewBag.Token = token;
            return View("/Views/FluenteDoc/Token.cshtml");
        }

        [HttpPost]
        [HttpGet]
        [Route("FluenteDoc/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Authorization");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("FluenteDoc/Authentication")]
        public async Task<IActionResult> LoginAsync(string email, string psw)
        {
            if (Setup.Config.Config.JwtInfo == null)
            {
                throw new InvalidOperationException("Use UseJwt at Arquitetura startup to set authentication parameters");
            }

            var authenticationUser = new FluenteAuthenticationUser
            {
                Email = email,
                Password = psw
            };

            var service = ServiceFactory.Create(Setup.Config.Config.JwtInfo.FluenteAuthenticationServiceType, HttpContext, "DocAuthenticationServiceType for FluenteDoc").FluenteCast<FluenteAuthenticationService>();
            var token = await service.LoginAsync(authenticationUser);
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Error trying to authenticate");
            }

            Response.Cookies.Append("Authorization", token, new CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
            return RedirectToAction(nameof(Token));
        }
    }
}
