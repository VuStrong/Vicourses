using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Quiz
{
    public class ChangeQuizzesOrderRequest
    {
        [Required]
        public List<string> QuizIds { get; set; } = [];
    }
}
