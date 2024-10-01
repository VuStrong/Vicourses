namespace CourseService.Domain.Models
{
    public class User : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? ThumbnailUrl { get; private set; }

        private User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public static User Create(string id, string name, string email, string? thumbnailUrl)
        {
            return new User(id, name, email)
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
