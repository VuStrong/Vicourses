using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CourseService.Application.Utils
{
    internal static class JwtHelper
    {
        public static string GenerateJWT(string secretKey, Action<SecurityTokenDescriptor> configureDescriptor)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var descriptor = new SecurityTokenDescriptor();
            descriptor.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            configureDescriptor.Invoke(descriptor);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
