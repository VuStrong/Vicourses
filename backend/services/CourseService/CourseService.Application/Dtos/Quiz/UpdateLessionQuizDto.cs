namespace CourseService.Application.Dtos.Quiz
{
    public class UpdateLessionQuizDto
    {
        public string Title { get; set; }
        public List<CreateUpdateLessionQuizAnswerDto> Answers { get; set; } = [];

        public UpdateLessionQuizDto(string title)
        {
            Title = title;
        }
    }
}
