using AutoMapper;
using CourseService.Application.Dtos.Category;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ICourseRepository courseRepository,
            ICategoryDomainService categoryDomainService,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _courseRepository = courseRepository;
            _categoryDomainService = categoryDomainService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto data)
        {
            Category? parent = null;
            if (data.ParentId != null)
            {
                parent = await _categoryRepository.FindOneAsync(c => c.Id == data.ParentId && c.ParentId == null) ??
                    throw new CategoryNotFoundException(data.ParentId);
            }

            var category = await _categoryDomainService.CreateAsync(data.Name, parent);

            await _categoryRepository.CreateAsync(category);

            _logger.LogInformation("A category was created with Id = {Id}", category.Id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            if (!(await _categoryRepository.ExistsAsync(categoryId)))
            {
                throw new CategoryNotFoundException(categoryId);
            }

            if (await _categoryRepository.ExistsAsync(c => c.ParentId == categoryId))
            {
                throw new ForbiddenException("This category cannot be deleted");
            }

            if (await ExistsCategoryInAnyCourses(categoryId))
            {
                throw new ForbiddenException("This category cannot be deleted");
            }

            await _categoryRepository.DeleteOneAsync(categoryId);

            _logger.LogInformation($"Deleted category: {categoryId}");
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync(GetCategoriesParamsDto? paramsDto = null)
        {
            var categories = await _categoryRepository.GetAllAsync(paramsDto?.Keyword, paramsDto?.ParentId);

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryBySlugAsync(string slug)
        {
            var category = await _categoryRepository.FindOneAsync(c => c.Slug == slug);

            if (category == null)
            {
                throw new CategoryNotFoundException(slug);
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<List<CategoryWithSubsDto>> GetRootCategoriesWithSubCategories()
        {
            var results = await _categoryRepository.GetRootCategoriesWithSubCategories();

            return _mapper.Map<List<CategoryWithSubsDto>>(results);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(string categoryId, UpdateCategoryDto data)
        {
            var category = await _categoryRepository.FindOneAsync(categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            await _categoryDomainService.UpdateAsync(category, data.Name ?? category.Name);

            await _categoryRepository.UpdateAsync(category);

            _logger.LogInformation($"Category {category.Id} updated");

            return _mapper.Map<CategoryDto>(category);
        }

        private async Task<bool> ExistsCategoryInAnyCourses(string categoryId)
        {
            return await _courseRepository.ExistsAsync(
                c => c.Category.Id == categoryId || c.SubCategory.Id == categoryId);
        }
    }
}
