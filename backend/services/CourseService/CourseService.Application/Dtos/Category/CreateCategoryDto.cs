namespace CourseService.Application.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string? ParentId { get; set; }

        public CreateCategoryDto(string name, string? parentId)
        {
            Name = name;
            ParentId = parentId;
        }
    }
}
