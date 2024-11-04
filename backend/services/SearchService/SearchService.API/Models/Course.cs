namespace SearchService.API.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);
    public record CategoryInCourse(string Id, string Name, string Slug);
    public record Locale(string Name, string EnglishTitle);

    public class Course
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleCleaned { get; set; } = string.Empty;
        public string[] LearnedContents { get; set; } = [];
        public string Level { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int StudentCount { get; set; }
        public Locale? Locale { get; set; }
        public string? ThumbnailUrl { get; set; }
        public UserInCourse User { get; set; } = null!;
        public CategoryInCourse Category { get; set; } = null!;
        public CategoryInCourse SubCategory { get; set; } = null!;
        public string[] Tags { get; set; } = [];
    }
}
