namespace CourseService.Application.Dtos.Quiz
{
    public class CreateLessonQuizDto
    {
        public string Title { get; set; }
        public string LessonId { get; set; }
        public string UserId { get; set; }
        public List<CreateUpdateLessonQuizAnswerDto> Answers { get; set; } = [];

        public CreateLessonQuizDto(string title, string lessonId, string userId)
        {
            Title = title;
            LessonId = lessonId;
            UserId = userId;
        }
    }

    public class CreateUpdateLessonQuizAnswerDto
    {
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
        public string? Explanation { get; set; }

        public CreateUpdateLessonQuizAnswerDto(string title, bool isCorrect, string? explanation)
        {
            Title = title;
            IsCorrect = isCorrect;
            Explanation = explanation;
        }
    }
}
