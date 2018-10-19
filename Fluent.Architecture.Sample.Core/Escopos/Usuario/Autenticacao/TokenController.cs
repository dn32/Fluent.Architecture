
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Util;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fluent.Architecture.Sample.Core.Escopos.Usuario.Autenticacao
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        [FluentAllowAnonymous]
        [HttpPost]
        public JsonResult Post(string username, string password)
        {
            if (CheckUser(username, password))
            {
                return Json( new { token = AutenticationUtil.GenerateToken(username) });
            }

            throw new UnauthorizedAccessException();
        }

        [HttpPut]
        public string Put()
        {
            return "value";
        }

        private bool CheckUser(string username, string password)
        {
            return true;
        }
    }
}
