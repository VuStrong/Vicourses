using CourseService.Domain.Enums;

namespace CourseService.Application.Dtos.Lession
{
    public class CreateLessionDto
    {
        public string Title { get; set; }
        public string CourseId { get; set; }
        public string SectionId { get; set; }
        public string UserId { get; set; }
        public LessionType Type { get; set; }
        public string? Description { get; set; }

        public CreateLessionDto(string title, string courseId, string sectionId, string userId, LessionType type, string? description)
        {
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            UserId = userId;
            Type = type;
            Description = description;
        }
    }
}
