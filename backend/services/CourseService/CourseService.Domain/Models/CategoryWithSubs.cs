namespace CourseService.Domain.Models
{
    public class CategoryWithSubs : Category
    {
        public IReadOnlyList<Category> SubCategories { get; private set; } = [];

        private CategoryWithSubs(string id, string name, string slug) : base(id, name, slug) { } 
    }
}
