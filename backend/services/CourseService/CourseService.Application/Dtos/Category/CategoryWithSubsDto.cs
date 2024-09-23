namespace CourseService.Application.Dtos.Category
{
    public class CategoryWithSubsDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public List<CategoryDto> SubCategories { get; set; } = [];
    }
}
