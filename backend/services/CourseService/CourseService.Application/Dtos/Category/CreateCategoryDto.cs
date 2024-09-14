namespace CourseService.Application.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = null!;
        public CategoryBannerDto? Banner { get; set; }

        public CreateCategoryDto(string name, CategoryBannerDto? banner)
        {
            Name = name;
            Banner = banner;
        }
    }
}
