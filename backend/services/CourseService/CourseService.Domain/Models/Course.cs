using CourseService.Domain.Constracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Utils;

namespace CourseService.Domain.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);

    public record CategoryInCourse(string Id, string Name, string Slug);

    public class Course : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string Title { get; set; } = null!;
        public string TitleCleaned { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> Tags { get; set; } = [];
        public List<string> Requirements { get; set; } = [];
        public List<string> TargetStudents { get; set; } = [];
        public List<string> LearnedContents { get; set; } = [];
        public CourseLevel Level { get; set; } = CourseLevel.All;
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int StudentCount { get; set; }
        public string? Language { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Unpublished;
        public ImageFile? Thumbnail { get; set; }
        public VideoFile? PreviewVideo { get; set; }
        public CategoryInCourse Category { get; set; } = null!;
        public UserInCourse User { get; set; } = null!;

        private Course() {}

        public static Course Create(string title, string? description, CategoryInCourse category, UserInCourse user)
        {
            return new Course()
            {
                Id = StringUtils.GenerateNumericIdString(8),
                Title = title,
                TitleCleaned = title.ToSlug(),
                Description = description,
                Category = category,
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
