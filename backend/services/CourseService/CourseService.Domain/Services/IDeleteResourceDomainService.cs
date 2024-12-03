using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface IDeleteResourceDomainService
    {
        Task SetSectionDeletedAsync(Section section);
        Task SetCategoryDeletedAsync(Category category);
    }
}
