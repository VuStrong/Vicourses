namespace CourseService.Application.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public ImageFileDto? Banner { get; set; }

        public UpdateCategoryDto() { }

        public UpdateCategoryDto(string? name, ImageFileDto? banner)
        {
            Name = name;
            Banner = banner;
        }
    }
}
