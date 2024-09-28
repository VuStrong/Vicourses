using CourseService.Application.Dtos.Quiz;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Quiz
{
    public class CreateLessionQuizRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; } = null!;

        public bool IsMultiChoice { get; set; }

        [Required]
        [Length(2, 5, ErrorMessage = "{0} length must be between {1} and {2}")]
        public List<CreateUpdateQuizAnswerRequest> Answers { get; set; } = [];

        public CreateLessionQuizDto ToCreateLessionQuizDto(string lessionId, string userId)
        {
            List<CreateUpdateLessionQuizAnswerDto> answersDto = [];

            foreach (var answers in Answers)
            {
                answersDto.Add(new CreateUpdateLessionQuizAnswerDto(
                    answers.Title,
                    answers.IsCorrect,
                    answers.Explanation
                ));
            }

            return new CreateLessionQuizDto(Title, lessionId, userId, IsMultiChoice)
            {
                Answers = answersDto
            };
        }
    }

    public class CreateUpdateQuizAnswerRequest
    {
        [Required]
        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Title { get; set; } = null!;

        public bool IsCorrect { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string? Explanation { get; set; }
    }
}
