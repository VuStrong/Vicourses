using CourseService.Application.Dtos.Category;

namespace CourseService.Application.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryBySlugAsync(string slug);

        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto data);
        Task<CategoryDto> UpdateCategoryAsync(string categoryId, UpdateCategoryDto data);

        Task DeleteCategoryAsync(string categoryId);
    }
}
