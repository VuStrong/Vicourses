using CourseService.Domain.Constracts;

namespace CourseService.Domain.Models
{
    public class User : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string Name { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
    }
}
