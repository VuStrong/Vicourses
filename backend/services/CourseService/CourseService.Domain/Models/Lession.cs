using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Lession : IBaseEntity
    {
        public string Id { get; protected set; }
        public string CourseId { get; set; }
        public string SectionId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order {  get; set; }
        public LessionType Type { get; set; } = LessionType.Video;
        public VideoFile? Video { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private Lession(string id, string title, string courseId, string sectionId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            UserId = userId;
        }

        public static Lession Create(string title, string courseId, string sectionId, string userId, LessionType type, string? description)
        {
            var id = StringExtensions.GenerateIdString(14);

            return new Lession(id, title, courseId, sectionId, userId)
            {
                Description = description,
                Type = type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null)
        {
            if (title != null) Title = title;
            if (description != null) Description = description;

            UpdatedAt = DateTime.Now;
        }
    }
}
