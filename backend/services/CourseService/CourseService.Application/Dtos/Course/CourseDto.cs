namespace CourseService.Application.Dtos.Course
{
    public record UserInCourseDto(string Id, string Name, string? ThumbnailUrl);

    public record CategoryInCourseDto(string Id, string Name, string Slug);

    public class CourseDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string TitleCleaned { get; set; } = null!;
        public string[] LearnedContents { get; set; } = [];
        public string Level { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int StudentCount { get; set; }
        public string? Language { get; set; }
        public string? ThumbnailUrl { get; set; }
        public UserInCourseDto User { get; set; } = null!;
        public CategoryInCourseDto Category { get; set; } = null!;
    }
}