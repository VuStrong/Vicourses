using CourseService.Domain.Contracts;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Section : IBaseEntity
    {
        public string Id { get; protected set; }
        public string CourseId { get; protected set; }
        public string UserId { get; protected set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

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
