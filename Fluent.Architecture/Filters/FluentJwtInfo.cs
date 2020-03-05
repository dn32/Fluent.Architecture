using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace dn32.infra.Filters
{
    public class InformacoesDoJWT
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public TimeSpan? Expires { get; set; }

        public Type DnAuthenticationServiceType { get; set; }

        public bool ValidateIssuerSigningKey { get; set; } = true;

        public bool ValidateLifetime { get; set; } = true;

        public bool ValidateIssuer { get; set; } = true;

        public bool ValidateAudience { get; set; } = true;

        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(30);

        internal SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.Default.GetBytes(SecretKey));

        internal SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha512);
    }
}
