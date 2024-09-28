using CourseService.Application.Dtos.Category;

namespace CourseService.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesAsync(GetCategoriesParamsDto? paramsDto = null);
        Task<List<CategoryWithSubsDto>> GetRootCategoriesWithSubCategories();
        Task<CategoryDto> GetCategoryBySlugAsync(string slug);

        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto data);
        Task<CategoryDto> UpdateCategoryAsync(string categoryId, UpdateCategoryDto data);

        Task DeleteCategoryAsync(string categoryId);
    }
}
