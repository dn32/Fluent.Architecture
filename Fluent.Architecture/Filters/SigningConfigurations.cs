using Microsoft.IdentityModel.Tokens;
using System;

namespace Fluente.Arquitetura.Filters
{
    public static class SigningConfigurations
    {
        private static FluenteJwtInfo Info => Setup.Config.Config.JwtInfo;

        internal static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = Info.SymmetricSecurityKey,
                ValidAudience = Info.Audience,
                ValidIssuer = Info.Issuer,
                ValidateIssuerSigningKey = Info.ValidateIssuerSigningKey,
                ValidateLifetime = Info.ValidateLifetime,
                ValidateIssuer = Info.ValidateIssuer,
                ValidateAudience = Info.ValidateAudience,
                ClockSkew = Info.ClockSkew
            };
        }
    }
}
