using CourseService.Domain.Contracts;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Category : IBaseEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public string? ParentId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected Category(string id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }

        public static Category Create(string name, string? parentId)
        {
            var id = StringExtensions.GenerateNumericIdString(6);

            return new Category(id, name, name.ToSlug())
            {
                ParentId = parentId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public void UpdateInfo(string name)
        {
            if (name != Name)
            {
                Name = name;
                Slug = name.ToSlug();
            }

            UpdatedAt = DateTime.Now;
        }
    }
}
