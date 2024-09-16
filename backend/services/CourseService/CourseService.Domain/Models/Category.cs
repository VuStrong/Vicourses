using CourseService.Domain.Constracts;
using CourseService.Domain.Utils;

namespace CourseService.Domain.Models
{
    public class Category : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Category Create(string name, string? parentId)
        {
            return new Category()
            {
                Id = StringUtils.GenerateNumericIdString(6),
                Name = name,
                Slug = name.ToSlug(),
                ParentId = parentId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public void UpdateInfo(string name, string? parentId)
        {
            if (name != Name)
            {
                Name = name;
                Slug = name.ToSlug();
            }

            ParentId = parentId;
            UpdatedAt = DateTime.Now;
        }
    }
}
