using CourseService.Application.Dtos.Lesson;

namespace CourseService.Application.Dtos.Section
{
    public class SectionWithLessonsDto
    {
        public string Id { get; private set; } = string.Empty;
        public string CourseId { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public int Order { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public List<LessonDto> Lessons { get; set; } = [];
        public int Duration { get; set; }
        public int LessonCount { get; set; }
    }
}
