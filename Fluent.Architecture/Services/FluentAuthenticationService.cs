using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Filters;
using Fluent.Architecture.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Services
{
    public abstract class FluentAuthenticationService : TransactionalService
    {
        public abstract Task<bool> AuthenticateAsync(FluentAuthenticationUser user);

        public virtual void Register(FluentAuthenticationUser user) { }

        public virtual async Task<string> LoginAsync(FluentAuthenticationUser user)
        {
            if (await AuthenticateAsync(user))
            {
                var identity = new ClaimsIdentity(new GenericIdentity(user.Email), new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")) });
                return GenerateToken(identity, TimeSpan.FromDays(1));
            }
            else
            {
                return string.Empty;
            }
        }

        protected virtual string GenerateToken(ClaimsIdentity identity, TimeSpan expires)
        {
            var now = DateTime.Now;
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Setup.Config.Config.JwtInfo.Issuer,
                Audience = Setup.Config.Config.JwtInfo.Audience,
                SigningCredentials = Setup.Config.Config.JwtInfo.SigningCredentials,
                Subject = identity,
                NotBefore = now,
                Expires = now.Add(expires)
            });

            return handler.WriteToken(securityToken);
        }
    }
}
