using CourseService.Domain.Constracts;
using CourseService.Domain.Utils;

namespace CourseService.Domain.Models
{
    public class Category : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public ImageFile? Banner { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Category Create(string name, ImageFile? banner)
        {
            return new Category()
            {
                Id = StringUtils.GenerateNumericIdString(6),
                Name = name,
                Slug = name.ToSlug(),
                Banner = banner,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public void UpdateInfo(string? name = null, ImageFile? banner = null)
        {
            if (name != null)
            {
                Name = name;
                Slug = name.ToSlug();
            }

            if (banner != null)
            {
                Banner = banner;
            }

            if (name != null || banner != null)
            {
                UpdatedAt = DateTime.Now;
            }
        }
    }
}
