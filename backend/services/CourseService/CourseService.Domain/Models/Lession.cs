using CourseService.Domain.Constracts;
using CourseService.Domain.Enums;

namespace CourseService.Domain.Models
{
    public class Lession : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string CourseId { get; set; } = null!;
        public string SectionId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order {  get; set; }
        public LessionType Type { get; set; } = LessionType.Video;
        public VideoFile? Video { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
