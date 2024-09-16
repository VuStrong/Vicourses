namespace CourseService.Application.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public bool SetRoot { get; set; }
    }
}
