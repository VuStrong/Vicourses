namespace CourseService.Application.Dtos.Lesson
{
    public class UpdateLessonQuizDto
    {
        public string UserId { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<CreateUpdateLessonQuizAnswerDto> Answers { get; set; } = [];
    }
}
