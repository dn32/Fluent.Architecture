using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace dn32.infra.Util
{
    public static class AuthenticationUtil
    {
        private static string Secret { get; set; }

        internal static void Initialize()
        {
            using (var hmac = new HMACSHA256())
            {
                //Secret = "KipzNHM2NUxLSElUWVJTNDY1NzY4NzAtOTBAIyQlKiomQClOTEtUTHM1c25iSkhHQ1tdeyE5NGFAQHNvaSlVSlNETEpLSMOib3AoQCMkJcKoJipqTTIzNDU2aEU1Vw==";
                Secret = Convert.ToBase64String(hmac.Key);
            }
        }

        //internal static bool ValidateToken(string token, out string username)
        //{
        //    username = null;

        //    var simplePrinciple = GetPrincipal(token);

        //    if (!(simplePrinciple?.Identity is ClaimsIdentity identity))
        //    {
        //        return false;
        //    }

        //    if (!identity.IsAuthenticated)
        //    {
        //        return false;
        //    }

        //    username = identity?.FindFirst(ClaimTypes.Name)?.Valor;

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return false;
        //    }

        //    More validate to check whether username exists in system

        //        return true;
        //}

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new AccessViolationException("Token não informado");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            if (!(tokenHandler.ReadToken(token) is JwtSecurityToken))
            {
                throw new AccessViolationException();
            }

            var symmetricKey = Convert.FromBase64String(Secret);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new TimeoutException("Expired token");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new AccessViolationException("Invalid Token");
            }
        }

        public static string GenerateToken(int id, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                            new Claim("id", id.ToString()),
                    }),

                Expires = now.AddMinutes(expireMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(stoken);
        }
    }
}