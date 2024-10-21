using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CourseService.Application.Dtos;
using System.Text;
using System.Security.Claims;
using CourseService.Application.Exceptions;

namespace CourseService.Application.Utils
{
    public class FileUploadValidator
    {
        private readonly byte[] _secretKey;

        public FileUploadValidator(string secretKey)
        {
            _secretKey = Encoding.UTF8.GetBytes(secretKey);
        }

        public UploadFileDto ValidateFileUploadToken(string token, string userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var parameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
                };

                var claims = tokenHandler.ValidateToken(token, parameters, out var _);

                var fileId = claims.FindFirstValue("fileId");
                var url = claims.FindFirstValue("url");
                var originalFileName = claims.FindFirstValue("originalFileName");
                var userIdInClaim = claims.FindFirstValue("userId");

                if (fileId == null || url == null || userIdInClaim == null || userIdInClaim != userId)
                {
                    throw new Exception();
                }

                return new UploadFileDto()
                {
                    FileId = fileId,
                    Url = url,
                    OriginalFileName = originalFileName ?? "",
                };   
            }
            catch (Exception)
            {
                throw new ForbiddenException("The upload token is invalid");
            }
        }
    }
}