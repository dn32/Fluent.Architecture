using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Filters;
using Fluent.Architecture.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Services
{
    public abstract class FluentAuthenticationService : TransactionalService
    {
        public abstract Task<(bool sucess, List<Claim> claims)> AuthenticateAsync(FluentAuthenticationUser user);

        public virtual void Register(FluentAuthenticationUser user) { }

        public virtual async Task<string> LoginAsync(FluentAuthenticationUser user)
        {
            if (string.IsNullOrWhiteSpace(user?.Email))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }

            var (sucess, claims) = await AuthenticateAsync(user);

            if (sucess)
            {
                if (claims == null) { claims = new List<Claim>(); }
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
                var identity = new ClaimsIdentity(new GenericIdentity(user.Email), claims);
                return GenerateToken(identity, Setup.Config.Config.JwtInfo.Expires ?? TimeSpan.FromDays(1));
            }
            else
            {
                return "Access denied";
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
