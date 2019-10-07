using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fluent.Architecture.Filters
{
    public static class SigningConfigurations
    {
        internal static FluentJwtInfo Info => Setup.Config.Config.JwtInfo;

        internal static string Issuer => Info.Issuer;

        internal static string Audience => Info.Audience;

        internal static string SecretKey => Info.SecretKey;

        internal static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = Info.SymmetricSecurityKey,
                ValidAudience = Audience,
                ValidIssuer = Issuer,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        }

        public static (JwtSecurityTokenHandler handler, SecurityToken securityToken) GetSecurityToken(ClaimsIdentity identity)
        {
            var createDate = DateTime.Now;
            var expirationDate = createDate.AddDays(1); //Duração de 1 dias
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = Info.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate,
            });

            return (handler, securityToken);
        }
    }
}
