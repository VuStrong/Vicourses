namespace CourseService.Application.Dtos.Quiz
{
    public class QuizDto
    {
        public string Id { get; set; } = string.Empty;
        public string LessionId { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsMultiChoice { get; set; }
        public List<QuizAnswerDto> Answers { get; set; } = [];
    }

    public class QuizAnswerDto
    {
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
    }
}
