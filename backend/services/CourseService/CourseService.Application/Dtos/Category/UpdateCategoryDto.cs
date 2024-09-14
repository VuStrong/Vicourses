namespace CourseService.Application.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public CategoryBannerDto? Banner { get; set; }

        public UpdateCategoryDto() { }

        public UpdateCategoryDto(string? name, CategoryBannerDto? banner)
        {
            Name = name;
            Banner = banner;
        }
    }
}
