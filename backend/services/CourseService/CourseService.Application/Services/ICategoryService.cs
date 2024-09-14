using CourseService.Application.Dtos.Category;

namespace CourseService.Application.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(string categoryId);

        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto data);
        Task<CategoryDto> UpdateCategoryAsync(string categoryId, UpdateCategoryDto data);

        Task DeleteCategoryAsync(string categoryId);
    }
}
