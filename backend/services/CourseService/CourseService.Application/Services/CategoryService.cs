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
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IDeleteResourceDomainService _deleteResourceDomainService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ICategoryDomainService categoryDomainService,
            IDeleteResourceDomainService deleteResourceDomainService,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _categoryDomainService = categoryDomainService;
            _deleteResourceDomainService = deleteResourceDomainService;
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
            var category = await _categoryRepository.FindOneAsync(categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            await _deleteResourceDomainService.SetCategoryDeletedAsync(category);

            await _categoryRepository.UpdateAsync(category);
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
    }
}
