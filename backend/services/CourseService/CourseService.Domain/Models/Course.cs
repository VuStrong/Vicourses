using CourseService.Domain.Enums;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);

    public record CategoryInCourse(string Id, string Name, string Slug);

    public class Course : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string TitleCleaned { get; private set; }
        public string? Description { get; private set; }
        public IReadOnlyList<string> Tags { get; private set; } = [];
        public IReadOnlyList<string> Requirements { get; private set; } = [];
        public IReadOnlyList<string> TargetStudents { get; private set; } = [];
        public IReadOnlyList<string> LearnedContents { get; private set; } = [];
        public CourseLevel Level { get; private set; } = CourseLevel.All;
        public bool IsPaid { get; private set; }
        public decimal Price { get; private set; }
        public decimal Rating { get; private set; }
        public int Duration { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public int StudentCount { get; private set; }
        public Locale? Locale { get; private set; }
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

        public static Course Create(string title, string? description, Category category, Category subCategory, User user)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(title, 5, 60, nameof(title));

            if (!category.IsRoot)
            {
                throw new DomainException("Main category must be root category");
            }

            if (!subCategory.IsChildOf(category))
            {
                throw new DomainException("SubCategory must be child of main category");
            }

            var id = StringExtensions.GenerateIdString(12);
            var categoryInCourse = new CategoryInCourse(category.Id, category.Name, category.Slug);
            var subCategoryInCourse = new CategoryInCourse(subCategory.Id, subCategory.Name, subCategory.Slug);
            var userInCourse = new UserInCourse(user.Id, user.Name, user.ThumbnailUrl);

            return new Course(id, title, title.ToSlug(), categoryInCourse, subCategoryInCourse, userInCourse)
            {
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null, List<string>? tags = null, List<string>? requirements = null,
            List<string>? targetStudents = null, List<string>? learnedContents = null, decimal? price = null, string? locale = null,
            Category? category = null, Category? subCategory = null, CourseLevel? level = null)
        {
            if (title != null)
            {
                title = title.Trim();
                DomainValidationException.ThrowIfStringOutOfLength(title, 5, 60, nameof(title));

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
                DomainValidationException.ThrowIfNegative(price ?? 0);

                Price = price ?? 0;
                IsPaid = price != 0;
            }

            if (locale != null) Locale = new Locale(locale);
            
            if (category != null && subCategory == null)
            {
                throw new DomainValidationException("SubCategory is required when main Category is set");
            }
            else if (category != null && !category.IsRoot)
            {
                throw new DomainException("Main category must be root category");
            }
            else if (subCategory != null && subCategory.ParentId != (category?.Id ?? Category.Id))
            {
                throw new DomainException("SubCategory must be child of main category");
            }

            if (category != null)
            {
                Category = new CategoryInCourse(category.Id, category.Name, category.Slug);
            }
            if (subCategory != null)
            {
                SubCategory = new CategoryInCourse(subCategory.Id, subCategory.Name, subCategory.Slug);
            }

            if (level != null) Level = level ?? CourseLevel.All;

            UpdatedAt = DateTime.Now;

            AddUniqueDomainEvent(new CourseInfoUpdatedDomainEvent(this));
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

            AddUniqueDomainEvent(new CoursePublishedDomainEvent(this));
        }

        public void CancelApproval(List<string>? reasons = null)
        {
            IsApproved = false;
            Status = CourseStatus.Unpublished;

            AddUniqueDomainEvent(new CourseUnpublishedDomainEvent(this));
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

            if (Status == CourseStatus.Published)
            {
                AddUniqueDomainEvent(new CoursePublishedDomainEvent(this));
            }
            else if (Status == CourseStatus.Unpublished)
            {
                AddUniqueDomainEvent(new CourseUnpublishedDomainEvent(this));
            }
        }

        public Enrollment EnrollStudent(string studentId)
        {
            if (Status != CourseStatus.Published)
            {
                throw new DomainException("Cannot enroll student to Unpublished course");
            }

            var enrollment = Enrollment.Create(Id, studentId);

            StudentCount++;

            AddDomainEvent(new UserEnrolledDomainEvent(studentId, this));

            return enrollment;
        }

        public void UpdateThumbnail(ImageFile image)
        {
            if (Thumbnail != null && Thumbnail.FileId == image.FileId) return;

            var oldThumb = Thumbnail;
            Thumbnail = image;

            UpdatedAt = DateTime.Now;

            AddUniqueDomainEvent(new CourseThumbnailUpdatedDomainEvent(this, oldThumb));
        }

        public void UpdatePreviewVideo(VideoFile video)
        {
            if (PreviewVideo != null && PreviewVideo.FileId == video.FileId) return;

            var oldVideo = PreviewVideo;
            PreviewVideo = video;

            UpdatedAt = DateTime.Now;
            
            AddUniqueDomainEvent(new CoursePreviewVideoUpdatedDomainEvent(this, oldVideo));
        }

        public void SetPreviewVideoStatusCompleted(string streamFileUrl, int duration)
        {
            if (PreviewVideo == null)
            {
                throw new DomainException("Cannot set video status because video is not set");
            }

            DomainValidationException.ThrowIfNegative(duration, nameof(duration));

            PreviewVideo = VideoFile.Create(
                PreviewVideo.FileId,
                PreviewVideo.Url,
                PreviewVideo.OriginalFileName,
                streamFileUrl,
                duration,
                VideoStatus.Processed
            );
        }

        public void SetPreviewVideoStatusFailed()
        {
            if (PreviewVideo == null)
            {
                throw new DomainException("Cannot set video status because video is not set");
            }

            PreviewVideo = VideoFile.Create(
                PreviewVideo.FileId,
                PreviewVideo.Url,
                PreviewVideo.OriginalFileName,
                status: VideoStatus.ProcessingFailed
            );
        }
    }
}
