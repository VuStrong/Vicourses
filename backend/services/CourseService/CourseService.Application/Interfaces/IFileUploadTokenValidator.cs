using CourseService.Application.Dtos;

namespace CourseService.Application.Interfaces
{
    public interface IFileUploadTokenValidator
    {
        UploadFileDto ValidateFileUploadToken(string token, string userId);
    }
}
