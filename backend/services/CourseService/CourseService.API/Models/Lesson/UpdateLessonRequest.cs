using System.ComponentModel.DataAnnotations;
using CourseService.Application.Dtos;
using CourseService.Application.Dtos.Lesson;

namespace CourseService.API.Models.Lesson
{
    public class UpdateLessonRequest
    {
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }

        public UpdateLessonVideoRequest? Video { get; set; }

        public UpdateLessonDto ToUpdateLessonDto()
        {
            return new UpdateLessonDto
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

    public record UpdateLessonVideoRequest
    {
        [Required(ErrorMessage = "The video.fileId field is required")]
        public string FileId { get; set; } = null!;

        [Required(ErrorMessage = "The video.url field is required")]
        public string Url { get; set; } = null!;

        [Required(ErrorMessage = "The video.fileName field is required")]
        public string FileName { get; set; } = null!;
    }
}
