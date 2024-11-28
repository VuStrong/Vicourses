using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Quiz
{
    public class ChangeQuizzesOrderRequest
    {
        [Required]
        public List<string> QuizIds { get; set; } = [];
    }
}
