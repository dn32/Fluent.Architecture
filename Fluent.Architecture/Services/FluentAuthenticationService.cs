using dn32.infra.Nucleo.Models;
using dn32.infra.servicos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace dn32.infra.Nucleo.Services
{
    public abstract class DnAuthenticationService : DnServicoTransacionalBase
    {
        public abstract Task<(bool sucess, List<Claim> claims)> AuthenticateAsync(DnAuthenticationUser user);

        public virtual void Register(DnAuthenticationUser user) { }

        public virtual async Task<string> LoginAsync(DnAuthenticationUser user)
        {
            if (user is null) { throw new ArgumentNullException(nameof(user)); }
            if (string.IsNullOrWhiteSpace(user.Email)) { throw new ArgumentNullException(nameof(user.Email)); }

            var (sucess, claims) = await AuthenticateAsync(user);

            if (sucess)
            {
                if (claims == null) { claims = new List<Claim>(); }
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
                var identity = new ClaimsIdentity(new GenericIdentity(user.Email), claims);
                return GenerateToken(identity, Setup.ConfiguracoesGlobais.InformacoesDoJWT.Expires ?? TimeSpan.FromDays(1));
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
                Issuer = Setup.ConfiguracoesGlobais.InformacoesDoJWT.Issuer,
                Audience = Setup.ConfiguracoesGlobais.InformacoesDoJWT.Audience,
                SigningCredentials = Setup.ConfiguracoesGlobais.InformacoesDoJWT.SigningCredentials,
                Subject = identity,
                NotBefore = now,
                Expires = now.Add(expires)
            });

            return handler.WriteToken(securityToken);
        }
    }
}
