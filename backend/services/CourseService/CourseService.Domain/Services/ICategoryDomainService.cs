using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface ICategoryDomainService
    {
        Task<Category> CreateAsync(string name, Category? parent);
        Task UpdateAsync(Category category, string name);
    }
}
