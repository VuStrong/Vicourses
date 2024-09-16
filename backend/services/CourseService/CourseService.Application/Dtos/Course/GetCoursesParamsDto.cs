namespace CourseService.Application.Dtos.Course
{
    public class GetCoursesParamsDto
    {
        public int Take { get; set; } = 10;
        public string? CategoryId { get; set; }
        public string? SubCategoryId { get; set; }
    }
}
