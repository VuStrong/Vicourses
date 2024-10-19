namespace CourseService.Application.Dtos.Lesson
{
    public class UpdateLessonDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public UpdateVideoFileDto? Video { get; set; }
    }
}
