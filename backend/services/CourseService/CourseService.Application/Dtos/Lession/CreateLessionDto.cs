namespace CourseService.Application.Dtos.Lession
{
    public class CreateLessionDto
    {
        public string Title { get; set; }
        public string CourseId { get; set; }
        public string SectionId { get; set; }
        public string? Description { get; set; }

        public CreateLessionDto(string title, string courseId, string sectionId, string? description)
        {
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            Description = description;
        }
    }
}
