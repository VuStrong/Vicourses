using CourseService.Domain.Constracts;

namespace CourseService.Domain.Models
{
    public class Section : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string CourseId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int LessonCount { get; set; }
        public int Duration { get; set; }
        public int Order {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
