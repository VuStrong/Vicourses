using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Org.BouncyCastle.OpenSsl;

namespace DiscountService.FunctionalTests
{
    internal static class JwtHelper
    {
        public static string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(GetRsaSecurityKeyFromFile("private.key"), SecurityAlgorithms.RsaSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var securityTokenHandler = new JwtSecurityTokenHandler();
            var token = securityTokenHandler.CreateToken(securityTokenDescriptor);
            var encodedAccessToken = securityTokenHandler.WriteToken(token);

            return encodedAccessToken;
        }

        private static RsaSecurityKey GetRsaSecurityKeyFromFile(string filePath)
        {
            var key = File.ReadAllText(filePath);

            if (key.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(key));
            }

            using var stringReader = new StringReader(key);
            var pemReader = new PemReader(stringReader);
            var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair!.Private);
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);
            
            return new RsaSecurityKey(rsa);
        }
    }
}
