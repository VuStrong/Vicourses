using CourseService.Domain.Exceptions;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Section : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Order { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Section(string id, string title, string courseId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            UserId = userId;
        }

        public static Section Create(string title, Course course, string? description)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(title, 3, 80, nameof(title));

            var id = StringExtensions.GenerateIdString(14);
            
            return new Section(id, title, course.Id, course.User.Id)
            {
                Description = description,
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
    }
}
