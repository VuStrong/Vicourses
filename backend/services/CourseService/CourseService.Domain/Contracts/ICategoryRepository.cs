using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllAsync(string? keyword = null, string? parentId = null);

        Task<List<CategoryWithSubs>> GetRootCategoriesWithSubCategories();
    }
}
