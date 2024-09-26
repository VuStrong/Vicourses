namespace CourseService.Domain.Objects
{
    public class Quiz
    {
        public string Title { get; set; } = string.Empty;
        public bool IsMultiChoice { get; set; }

        public List<Answer> Answers { get; set; } = [];
    }

    public class Answer
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }
    }
}
