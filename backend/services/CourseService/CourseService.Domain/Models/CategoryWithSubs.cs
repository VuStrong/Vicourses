namespace CourseService.Domain.Models
{
    public class CategoryWithSubs
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public IEnumerable<Category> SubCategories { get; set; } = [];
    }
}
