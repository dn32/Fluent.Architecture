using dn32.infra.Factory;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using dn32.infra.extensoes;

namespace dn32.infra.Nucleo.Doc.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        [HttpGet]
        [Route("DnDoc/Authentication")]
        public IActionResult Index()
        {
            if (Setup.ConfiguracoesGlobais.InformacoesDoJWT == null)
            {
                throw new InvalidOperationException("Use UseJwt at Arquitetura startup to set authentication parameters");
            }

            return View("/Views/DnDoc/Authentication.cshtml");
        }

        [HttpGet]
        [Route("DnDoc/Token")]
        public IActionResult Token()
        {
            var token = Request.Cookies["Authorization"];
            if (string.IsNullOrWhiteSpace(token)) { return RedirectToAction(nameof(Index)); }
            ViewBag.Token = token;
            return View("/Views/DnDoc/Token.cshtml");
        }

        [HttpPost]
        [HttpGet]
        [Route("DnDoc/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Authorization");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("DnDoc/Authentication")]
        public async Task<IActionResult> LoginAsync(string email, string psw)
        {
            if (Setup.ConfiguracoesGlobais.InformacoesDoJWT == null)
            {
                throw new InvalidOperationException("Use UseJwt at Arquitetura startup to set authentication parameters");
            }

            var authenticationUser = new DnAuthenticationUser
            {
                Email = email,
                Password = psw
            };

            var service = ServiceFactory.Create(Setup.ConfiguracoesGlobais.InformacoesDoJWT.DnAuthenticationServiceType, HttpContext, "DocAuthenticationServiceType for DnDoc").DnCast<DnAuthenticationService>();
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
