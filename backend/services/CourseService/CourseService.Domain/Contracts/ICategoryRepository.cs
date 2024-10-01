using CourseService.Domain.Models;
using CourseService.Domain.Objects;

namespace CourseService.Domain.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllAsync(string? keyword = null, string? parentId = null);

        Task<List<CategoryWithSubs>> GetRootCategoriesWithSubCategories();
    }
}
