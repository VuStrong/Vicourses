using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);

    public record CategoryInCourse(string Id, string Name, string Slug);

    public class Course : IBaseEntity
    {
        public string Id { get; protected set; }
        public string Title { get; set; }
        public string TitleCleaned { get; set; }
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
        public CategoryInCourse Category { get; set; }
        public CategoryInCourse SubCategory { get; set; }
        public UserInCourse User { get; set; }

        private Course(string id, string title, string titleCleaned, CategoryInCourse category, CategoryInCourse subCategory, UserInCourse user)
        {
            Id = id;
            Title = title;
            TitleCleaned = titleCleaned;
            Category = category;
            SubCategory = subCategory;
            User = user;
        }

        public static Course Create(string title, string? description, CategoryInCourse category, CategoryInCourse subCategory, UserInCourse user)
        {
            var id = StringExtensions.GenerateIdString(12);

            return new Course(id, title, title.ToSlug(), category, subCategory, user)
            {
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null, List<string>? tags = null, List<string>? requirements = null,
            List<string>? targetStudents = null, List<string>? learnedContents = null, decimal? price = null, string? language = null,
            ImageFile? thumbnail = null, VideoFile? previewVideo = null, CategoryInCourse? category = null, CategoryInCourse? subCategory = null,
            CourseLevel? level = null)
        {
            if (title != null)
            {
                Title = title;
                TitleCleaned = title.ToSlug();
            }

            if (description != null) Description = description;

            if (tags != null) Tags = tags;

            if (requirements != null) Requirements = requirements;

            if (targetStudents != null) TargetStudents = targetStudents;

            if (learnedContents != null) LearnedContents = learnedContents;

            if (price != null)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(price ?? 0);

                Price = price ?? 0;
                IsPaid = price != 0;
            }

            if (language != null) Language = language;
            
            if (thumbnail != null) Thumbnail = thumbnail;

            if (previewVideo != null) PreviewVideo = previewVideo;

            if (category != null) Category = category;

            if (subCategory != null) SubCategory = subCategory;

            if (level != null) Level = level ?? CourseLevel.All;

            UpdatedAt = DateTime.Now;
        }
    }
}
