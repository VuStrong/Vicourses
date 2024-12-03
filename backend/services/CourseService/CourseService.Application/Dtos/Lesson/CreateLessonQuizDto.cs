namespace CourseService.Application.Dtos.Lesson
{
    public class CreateLessonQuizDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public List<CreateUpdateLessonQuizAnswerDto> Answers { get; set; } = [];

        public CreateLessonQuizDto(string userId, string title)
        {
            UserId = userId;
            Title = title;
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
