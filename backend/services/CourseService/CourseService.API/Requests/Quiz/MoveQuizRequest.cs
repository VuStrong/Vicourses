using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Quiz
{
    public class MoveQuizRequest
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public int To { get; set; }
    }
}
