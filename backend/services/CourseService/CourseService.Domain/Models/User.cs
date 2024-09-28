using CourseService.Domain.Contracts;

namespace CourseService.Domain.Models
{
    public class User : IBaseEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string? ThumbnailUrl { get; private set; }

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

        public void UpdateInfoIgnoreNull(string? name = null, string? thumbnailUrl = null)
        {
            if (name != null) Name = name;
            if (thumbnailUrl != null) ThumbnailUrl = thumbnailUrl;
        }
    }
}
