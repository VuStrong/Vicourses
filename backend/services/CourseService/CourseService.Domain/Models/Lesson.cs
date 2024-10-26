using CourseService.Domain.Enums;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Lesson : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string SectionId { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Order {  get; private set; }
        public LessonType Type { get; private set; } = LessonType.Video;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public VideoFile? Video { get; private set; }

        #pragma warning disable CS8618
        private Lesson() { }

        private Lesson(string id, string title, string courseId, string sectionId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            UserId = userId;
        }

        public static Lesson Create(string title, Course course, Section section, LessonType type, string? description)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(title, 3, 80, nameof(title));

            if (section.CourseId != course.Id)
            {
                throw new DomainValidationException($"Section {section.Id} must be asset of the course {course.Id}");
            }

            var id = StringExtensions.GenerateIdString(14);

            return new Lesson(id, title, course.Id, section.Id, course.User.Id)
            {
                Description = description,
                Type = type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null)
        {
            if (title != null)
            {
                title = title.Trim();
                DomainValidationException.ThrowIfStringOutOfLength(title, 3, 80, nameof(title));

                Title = title;
            }

            if (description != null) Description = description;

            UpdatedAt = DateTime.Now;
        }

        public void UpdateVideo(VideoFile video)
        {
            if (Type != LessonType.Video)
            {
                throw new DomainException("Cannot update video of non-video lesson");
            }

            if (Video != null && Video.FileId == video.FileId) return;

            var oldVideo = Video;
            Video = video;

            UpdatedAt = DateTime.Now;
            
            AddUniqueDomainEvent(new LessonVideoUpdatedDomainEvent(this, oldVideo));
        }

        public void SetVideoStatusCompleted(string streamFileUrl, int duration)
        {
            if (Video == null)
            {
                throw new DomainException("Cannot set video status because video is not set");
            }

            DomainValidationException.ThrowIfNegative(duration, nameof(duration));

            Video = VideoFile.Create(
                Video.FileId,
                Video.Url,
                Video.OriginalFileName,
                streamFileUrl,
                duration,
                VideoStatus.Processed
            );
        }

        public void SetVideoStatusFailed()
        {
            if (Video == null)
            {
                throw new DomainException("Cannot set video status because video is not set");
            }

            Video = VideoFile.Create(
                Video.FileId,
                Video.Url,
                Video.OriginalFileName,
                status: VideoStatus.ProcessingFailed
            );
        }
    }
}
