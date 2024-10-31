using EventBus;
using SearchService.API.Models;

namespace SearchService.API.Application.IntegrationEvents.Course
{
    public class CourseInfoUpdatedIntegrationEvent : IntegrationEvent
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int StudentCount { get; set; }
        public Locale? Locale { get; set; }
        public string? ThumbnailUrl { get; set; }
        public UserInCourse User { get; set; } = null!;
        public CategoryInCourse Category { get; set; } = null!;
        public CategoryInCourse SubCategory { get; set; } = null!;
        public string? Description { get; set; }
        public string[] Tags { get; set; } = [];
        public string[] Requirements { get; set; } = [];
        public string[] TargetStudents { get; set; } = [];
    }
}
