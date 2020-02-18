using Microsoft.IdentityModel.Tokens;

namespace dn32.infra.Filters
{
    public static class SigningConfigurations
    {
        private static DnJwtInfo Info => Setup.Config.Config.JwtInfo;

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
