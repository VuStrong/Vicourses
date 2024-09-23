namespace CourseService.Application.Dtos.Section
{
    public class CreateSectionDto
    {
        public string Title { get; set; }
        public string CourseId { get; set; }
        public string? Description { get; set; }

        public CreateSectionDto(string title, string courseId, string? description)
        {
            Title = title;
            CourseId = courseId;
            Description = description;
        }
    }
}
