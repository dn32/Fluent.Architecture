using Microsoft.IdentityModel.Tokens;

namespace dn32.infra.Filters
{
    public static class SigningConfigurations
    {
        private static InformacoesDoJWT Info => Setup.ConfiguracoesGlobais.InformacoesDoJWT;

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
