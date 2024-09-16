namespace CourseService.Application.Dtos.Course
{
    public class GetCoursesParamsDto
    {
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public string? SearchKeyword { get; set; }
        public string? CategoryId { get; set; }
        public string? SubCategoryId { get; set; }
    }
}
