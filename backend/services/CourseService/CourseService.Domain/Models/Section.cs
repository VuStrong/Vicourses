using CourseService.Domain.Contracts;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Section : IBaseEntity
    {
        public string Id { get; protected set; }
        public string CourseId { get; protected set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected Section(string id, string title, string courseId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
        }

        public static Section Create(string title, string courseId, string? description)
        {
            var id = StringExtensions.GenerateIdString(14);
            
            return new Section(id, title, courseId)
            {
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
