using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Lession : IBaseEntity
    {
        public string Id { get; protected set; }
        public string CourseId { get; set; }
        public string SectionId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order {  get; set; }
        public LessionType Type { get; set; } = LessionType.Video;
        public VideoFile? Video { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private Lession(string id, string title, string courseId, string sectionId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
        }

        public static Lession Create(string title, string courseId, string sectionId, string? description)
        {
            var id = StringExtensions.GenerateIdString(14);

            return new Lession(id, title, courseId, sectionId)
            {
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
