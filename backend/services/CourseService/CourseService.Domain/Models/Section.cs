using CourseService.Domain.Contracts;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Section : IBaseEntity
    {
        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Order { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected Section(string id, string title, string courseId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            UserId = userId;
        }

        public static Section Create(string title, string courseId, string userId, string? description)
        {
            var id = StringExtensions.GenerateIdString(14);
            
            return new Section(id, title, courseId, userId)
            {
                Description = description,
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
