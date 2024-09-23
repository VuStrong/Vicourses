namespace CourseService.Domain.Models
{
    public class CategoryWithSubs
    {
        public string Id { get; protected set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Category> SubCategories { get; set; } = [];
    }
}
