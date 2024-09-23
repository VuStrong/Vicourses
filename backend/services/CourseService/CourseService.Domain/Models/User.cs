using CourseService.Domain.Contracts;

namespace CourseService.Domain.Models
{
    public class User : IBaseEntity
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public string? ThumbnailUrl { get; set; }

        private User(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static User Create(string id, string name, string? thumbnailUrl)
        {
            return new User(id, name)
            {
                ThumbnailUrl = thumbnailUrl
            };
        }
    }
}
