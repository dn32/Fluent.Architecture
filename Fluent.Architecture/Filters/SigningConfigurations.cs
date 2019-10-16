using Microsoft.IdentityModel.Tokens;
using System;

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
    }
}
