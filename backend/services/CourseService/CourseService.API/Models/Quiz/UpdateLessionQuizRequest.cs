using CourseService.Application.Dtos.Quiz;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Quiz
{
    public class UpdateLessionQuizRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        [Length(2, 5, ErrorMessage = "{0} length must be between {1} and {2}")]
        public List<CreateUpdateQuizAnswerRequest> Answers { get; set; } = [];

        public UpdateLessionQuizDto ToUpdateLessionQuizDto()
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

            return new UpdateLessionQuizDto(Title)
            {
                Answers = answersDto
            };
        }
    }
}
