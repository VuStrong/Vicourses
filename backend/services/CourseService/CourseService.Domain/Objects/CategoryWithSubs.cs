using CourseService.Domain.Models;

namespace CourseService.Domain.Objects
{
    public class CategoryWithSubs
    {
        public string Id { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string? ParentId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public IReadOnlyList<Category> SubCategories { get; private set; } = [];
    }
}
