namespace CourseService.Application.Dtos.Category
{
    public class GetCategoriesParamsDto
    {
        public string? Keyword { get; set; }
        public string? ParentId { get; set; }
    }
}
