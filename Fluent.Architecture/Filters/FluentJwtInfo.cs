using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Fluent.Architecture.Filters
{
    public class FluentJwtInfo
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public Type FluentAuthenticationServiceType { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.Default.GetBytes(SecretKey));

        public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha512);
    }
}
