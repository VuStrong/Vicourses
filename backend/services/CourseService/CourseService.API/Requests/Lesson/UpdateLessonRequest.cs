using System.ComponentModel.DataAnnotations;
using CourseService.Application.Dtos;
using CourseService.Application.Dtos.Lesson;

namespace CourseService.API.Requests.Lesson
{
    public class UpdateLessonRequest
    {
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }

        public string? VideoToken { get; set; }

        public UpdateLessonDto ToUpdateLessonDto()
        {
            return new UpdateLessonDto
            {
                Title = Title,
                Description = Description,
                VideoToken = VideoToken,
            };
        }
    }
}
