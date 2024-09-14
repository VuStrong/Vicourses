using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

namespace CourseService.API.Utils
{
    public static class TokenHelper
    {
        public static TokenValidationParameters GetTokenValidationParameters(string path)
        {
            var key = File.ReadAllText(path);

            if (key.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(key));
            }

            var rs256Token = key.Replace("-----BEGIN PUBLIC KEY-----", "");
            rs256Token = rs256Token.Replace("-----END PUBLIC KEY-----", "");
            rs256Token = rs256Token.Replace("\n", "");

            var keyBytes = Convert.FromBase64String(rs256Token);

            var asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            var rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            var rsaParameters = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };
            var rsa = new RSACryptoServiceProvider();

            rsa.ImportParameters(rsaParameters);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = false,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new RsaSecurityKey(rsa),
            };

            return validationParameters;
        }
    }
}
