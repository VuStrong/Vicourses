using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);

    public record CategoryInCourse(string Id, string Name, string Slug);

    public class Course : IBaseEntity
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string TitleCleaned { get; private set; }
        public string? Description { get; private set; }
        public List<string> Tags { get; private set; } = [];
        public List<string> Requirements { get; private set; } = [];
        public List<string> TargetStudents { get; private set; } = [];
        public List<string> LearnedContents { get; private set; } = [];
        public CourseLevel Level { get; private set; } = CourseLevel.All;
        public bool IsPaid { get; private set; }
        public decimal Price { get; private set; }
        public decimal Rating { get; private set; }
        public int Duration { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public int StudentCount { get; private set; }
        public string? Language { get; private set; }
        public bool IsApproved { get; private set; }
        public CourseStatus Status { get; private set; } = CourseStatus.Unpublished;
        public ImageFile? Thumbnail { get; private set; }
        public VideoFile? PreviewVideo { get; private set; }
        public CategoryInCourse Category { get; private set; }
        public CategoryInCourse SubCategory { get; private set; }
        public UserInCourse User { get; private set; }

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

        public void Approve()
        {
            if (Status == CourseStatus.Published) return;

            if (Status != CourseStatus.WaitingToVerify)
            {
                throw new DomainException("Cours's status must be WaitingToVerify to be published");
            }

            IsApproved = true;
            Status = CourseStatus.Published;
        }

        public void CancelApproval()
        {
            IsApproved = false;
            Status = CourseStatus.Unpublished;
        }

        public void SetStatus(CourseStatus status)
        {
            if (status == CourseStatus.Published && !IsApproved)
            {
                throw new DomainException("Course must be approved to be published");
            }
            else if (status == CourseStatus.WaitingToVerify && IsApproved)
            {
                throw new DomainException("Cannot set status to WaitingToVerify because course is already approved");
            }

            Status = status;
        }
    }
}
