namespace CourseService.Application.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = null!;
        public ImageFileDto? Banner { get; set; }

        public CreateCategoryDto(string name, ImageFileDto? banner)
        {
            Name = name;
            Banner = banner;
        }
    }
}
