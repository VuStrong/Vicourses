using CourseService.Domain.Events.User;

namespace CourseService.Domain.Models
{
    public class User : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? ThumbnailUrl { get; private set; }
        public bool EnrolledCoursesVisible { get; private set; } = true;

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

        public void UpdateInfoIgnoreNull(string? name = null, string? thumbnailUrl = null, bool? enrolledCoursesVisible = null)
        {
            bool updated = false;
            bool nameOrThumbnailUpdated = false;

            if (name != null && Name != name)
            {
                Name = name;
                nameOrThumbnailUpdated = true;
                updated = true;
            }

            if (thumbnailUrl != null && ThumbnailUrl != thumbnailUrl)
            {
                ThumbnailUrl = thumbnailUrl;
                nameOrThumbnailUpdated = true;
                updated = true;
            }

            if (enrolledCoursesVisible != null && enrolledCoursesVisible.Value != EnrolledCoursesVisible)
            {
                EnrolledCoursesVisible = enrolledCoursesVisible.Value;
                updated = true;
            }

            if (updated)
            {
                AddUniqueDomainEvent(new UserInfoUpdatedDomainEvent(this)
                {
                    NameOrThumbnailUpdated = nameOrThumbnailUpdated,
                });
            }
        }
    }
}
