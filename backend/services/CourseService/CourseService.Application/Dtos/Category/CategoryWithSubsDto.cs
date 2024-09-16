namespace CourseService.Application.Dtos.Category
{
    public class CategoryWithSubsDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public List<CategoryDto> SubCategories { get; set; } = [];
    }
}
