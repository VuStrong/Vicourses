using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CourseService.Application.Dtos;
using System.Text;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;

namespace CourseService.Application.Services
{
    public class FileUploadTokenValidator : IFileUploadTokenValidator
    {
        private readonly byte[] _secretKey;

        public FileUploadTokenValidator(string secretKey)
        {
            _secretKey = Encoding.UTF8.GetBytes(secretKey);
        }

        public UploadFileDto ValidateFileUploadToken(string token, string userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
                };

                var claims = tokenHandler.ValidateToken(token, parameters, out var _);

                var fileId = claims.FindFirst("fileId")?.Value;
                var url = claims.FindFirst("url")?.Value;
                var originalFileName = claims.FindFirst("originalFileName")?.Value;
                var userIdInClaim = claims.FindFirst("userId")?.Value;

                if (
                    string.IsNullOrEmpty(fileId) ||
                    string.IsNullOrEmpty(url) ||
                    string.IsNullOrEmpty(userIdInClaim) ||
                    userIdInClaim != userId)
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