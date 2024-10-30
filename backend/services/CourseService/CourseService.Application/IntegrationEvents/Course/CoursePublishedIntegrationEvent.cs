using CourseService.Application.Dtos.Course;
using EventBus;

namespace CourseService.Application.IntegrationEvents.Course
{
    public class CoursePublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleCleaned { get; set; } = string.Empty;
        public string[] LearnedContents { get; set; } = [];
        public string Level { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
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
        public CategoryInCourseDto SubCategory { get; set; } = null!;
        public string? Description { get; set; }
        public string[] Tags { get; set; } = [];
    }
}
