namespace CourseService.Application.Dtos.Lesson
{
    public class QuizDto
    {
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
