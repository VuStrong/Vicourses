namespace CourseService.Domain.Models
{
    public class CategoryWithSubs
    {
        public string Id { get; protected set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Category> SubCategories { get; set; } = [];
    }
}
