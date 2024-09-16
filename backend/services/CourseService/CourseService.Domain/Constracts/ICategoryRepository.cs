using CourseService.Domain.Models;

namespace CourseService.Domain.Constracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllAsync(string? keyword = null, string? parentId = null);

        Task<List<CategoryWithSubs>> GetRootCategoriesWithSubCategories();
    }
}
