namespace CourseService.Application.Dtos.Quiz
{
    public class CreateLessionQuizDto
    {
        public string Title { get; set; }
        public string LessionId { get; set; }
        public string UserId { get; set; }
        public bool IsMultiChoice { get; set; }
        public List<CreateUpdateLessionQuizAnswerDto> Answers { get; set; } = [];

        public CreateLessionQuizDto(string title, string lessionId, string userId, bool isMultiChoice = false)
        {
            Title = title;
            LessionId = lessionId;
            UserId = userId;
            IsMultiChoice = isMultiChoice;
        }
    }

    public class CreateUpdateLessionQuizAnswerDto
    {
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }

        public CreateUpdateLessionQuizAnswerDto(string title, bool isCorrect, string? explanation)
        {
            Title = title;
            IsCorrect = isCorrect;
            Explanation = explanation;
        }
    }
}
