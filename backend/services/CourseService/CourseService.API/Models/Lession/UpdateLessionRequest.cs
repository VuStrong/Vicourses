using System.ComponentModel.DataAnnotations;
using CourseService.Application.Dtos;
using CourseService.Application.Dtos.Lession;

namespace CourseService.API.Models.Lession
{
    public class UpdateLessionRequest
    {
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }

        public UpdateLessionVideoRequest? Video { get; set; }

        public UpdateLessionDto ToUpdateLessionDto()
        {
            return new UpdateLessionDto
            {
                Title = Title,
                Description = Description,
                Video = Video != null ? new UpdateVideoFileDto
                {
                    FileId = Video.FileId,
                    Url = Video.Url,
                    FileName = Video.FileName,
                } : null
            };
        }
    }

    public record UpdateLessionVideoRequest
    {
        [Required(ErrorMessage = "The video.fileId field is required")]
        public string FileId { get; set; } = null!;

        [Required(ErrorMessage = "The video.url field is required")]
        public string Url { get; set; } = null!;

        [Required(ErrorMessage = "The video.fileName field is required")]
        public string FileName { get; set; } = null!;
    }
}
