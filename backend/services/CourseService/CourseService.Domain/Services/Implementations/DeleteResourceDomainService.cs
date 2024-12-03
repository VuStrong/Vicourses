using CourseService.Domain.Contracts;
using CourseService.Domain.Events.Category;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;

namespace CourseService.Domain.Services.Implementations
{
    public class DeleteResourceDomainService : IDeleteResourceDomainService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteResourceDomainService(
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            ICategoryRepository categoryRepository)
        {
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task SetSectionDeletedAsync(Section section)
        {
            if ((await _lessonRepository.CountBySectionIdAsync(section.Id)) > 0)
            {
                throw new BusinessRuleViolationException("This section cannot be deleted because it contains lessons");
            }

            section.AddUniqueDomainEvent(new SectionDeletedDomainEvent(section));
        }

        public async Task SetCategoryDeletedAsync(Category category)
        {
            if (await _categoryRepository.ExistsAsync(c => c.ParentId == category.Id))
            {
                throw new BusinessRuleViolationException("This category cannot be deleted");
            }

            if (await ExistsCategoryInAnyCourses(category.Id))
            {
                throw new BusinessRuleViolationException("This category cannot be deleted");
            }

            category.AddUniqueDomainEvent(new CategoryDeletedDomainEvent(category));
        }

        private async Task<bool> ExistsCategoryInAnyCourses(string categoryId)
        {
            return await _courseRepository.ExistsAsync(
                c => c.Category.Id == categoryId || c.SubCategory.Id == categoryId);
        }
    }
}
