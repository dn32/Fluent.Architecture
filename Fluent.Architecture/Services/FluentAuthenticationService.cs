using Fluent.Architecture.Filters;
using Fluent.Architecture.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Fluent.Architecture.Core.Services
{
    public abstract class FluentAuthenticationService : TransactionalService
    {
        public abstract bool InternalLogin(string user, string psw);

        public virtual string Login(string user, string psw)
        {
            if (InternalLogin(user, psw))
            {
                var identity = new ClaimsIdentity(new GenericIdentity(user), new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")) });
                var (handler, securityToken) = SigningConfigurations.GetSecurityToken(identity);
                return handler.WriteToken(securityToken);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
