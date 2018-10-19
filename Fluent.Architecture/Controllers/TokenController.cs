
#if NETCOREAPP2_1

using System;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Util;
using Microsoft.AspNetCore.Mvc;

namespace Fluent.Architecture.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        [FluentAllowAnonymous]
        [HttpPost]
        public string Post(string username, string password)
        {
            if (CheckUser(username, password))
            {
                return AutenticationUtil.GenerateToken(username);
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

#endif
