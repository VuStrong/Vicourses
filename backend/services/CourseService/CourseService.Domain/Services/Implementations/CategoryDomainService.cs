using CourseService.Domain.Contracts;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;

namespace CourseService.Domain.Services.Implementations
{
    public class CategoryDomainService : ICategoryDomainService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseRepository _courseRepository;

        public CategoryDomainService(ICategoryRepository categoryRepository, ICourseRepository courseRepository)
        {
            _categoryRepository = categoryRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Category> CreateAsync(string name, Category? parent)
        {
            var category = Category.Create(name, parent);

            if (await _categoryRepository.ExistsAsync(c => c.Slug == category.Slug))
            {
                throw new BusinessRuleViolationException($"Category {category.Name} with slug {category.Slug} already exists");
            }

            return category;
        }

        public async Task UpdateAsync(Category category, string name)
        {
            if (await ExistsCategoryInAnyCourses(category.Id))
            {
                throw new BusinessRuleViolationException($"Cannot update category {category.Id} because it already in use by courses");
            }

            category.UpdateInfo(name);
        }

        private async Task<bool> ExistsCategoryInAnyCourses(string categoryId)
        {
            return await _courseRepository.ExistsAsync(
                c => c.Category.Id == categoryId || c.SubCategory.Id == categoryId);
        }
    }
}
