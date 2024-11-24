using CourseService.Domain.Exceptions;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Category : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public string? ParentId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public bool IsRoot { get => ParentId == null; }

        #pragma warning disable CS8618
        private Category() { }

        private Category(string id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }

        internal static Category Create(string name, Category? parent)
        {
            name = name.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(name, 2, 40, nameof(name));

            if (parent != null && !parent.IsRoot)
            {
                throw new BusinessRuleViolationException("Parent category must be root category");
            }

            var id = StringExtensions.GenerateNumericIdString(6);

            return new Category(id, name, name.ToSlug())
            {
                ParentId = parent?.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        internal void UpdateInfo(string name)
        {
            name = name.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(name, 2, 40, nameof(name));

            if (name != Name)
            {
                Name = name;
                Slug = name.ToSlug();
            }

            UpdatedAt = DateTime.Now;
        }

        public bool IsChildOf(Category other)
        {
            return ParentId == other.Id;
        }
    }
}
