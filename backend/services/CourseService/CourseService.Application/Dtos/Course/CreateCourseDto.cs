namespace CourseService.Application.Dtos.Course
{
    public class CreateCourseDto
    {
        public string Title { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? Description { get; set; }

        public CreateCourseDto(string title, string categoryId, string userId, string? description)
        {
            Title = title;
            CategoryId = categoryId;
            UserId = userId;
            Description = description;
        }
    }
}