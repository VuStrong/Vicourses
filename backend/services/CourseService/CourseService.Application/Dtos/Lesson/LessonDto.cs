namespace CourseService.Application.Dtos.Lesson
{
    public class LessonDto
    {
        public string Id { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string SectionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Description { get; set; }
        public VideoFileDto? Video { get; set; }
        public int QuizzesCount { get; set; }
        public List<QuizDto> Quizzes { get; set; } = [];
    }
}
