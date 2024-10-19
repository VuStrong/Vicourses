namespace CourseService.Application.Dtos.Quiz
{
    public class UpdateLessonQuizDto
    {
        public string Title { get; set; }
        public List<CreateUpdateLessonQuizAnswerDto> Answers { get; set; } = [];

        public UpdateLessonQuizDto(string title)
        {
            Title = title;
        }
    }
}
