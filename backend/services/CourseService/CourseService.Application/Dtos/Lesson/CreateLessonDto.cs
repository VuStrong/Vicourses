using CourseService.Domain.Enums;

namespace CourseService.Application.Dtos.Lesson
{
    public class CreateLessonDto
    {
        public string Title { get; set; }
        public string CourseId { get; set; }
        public string SectionId { get; set; }
        public string UserId { get; set; }
        public LessonType Type { get; set; }
        public string? Description { get; set; }

        public CreateLessonDto(string title, string courseId, string sectionId, string userId, LessonType type, string? description)
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
