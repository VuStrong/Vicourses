﻿using AutoMapper;
using CourseService.Application.Dtos.Category;
using CourseService.Application.Exceptions;
using CourseService.Domain.Constracts;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ICourseRepository courseRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto data)
        {
            ImageFile? banner = data.Banner != null ? new ImageFile()
            {
                FileId = data.Banner.FileId,
                Url = data.Banner.Url,
            } : null;

            var category = Category.Create(data.Name, banner);

            if (await _categoryRepository.ExistAsync(c => c.Slug == category.Slug))
            {
                throw new BadRequestException($"Category {category.Name} already exists");
            }

            await _categoryRepository.CreateAsync(category);

            _logger.LogInformation("A category was created with Id = {Id}", category.Id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            if (await _courseRepository.ExistAsync(c => c.Category.Id == categoryId))
            {
                throw new InternalServerException("This category could not be deleted");
            }

            if (!(await _categoryRepository.ExistAsync(c => c.Id == categoryId)))
            {
                throw new CategoryNotFoundException(categoryId);
            }

            await _categoryRepository.DeleteOneAsync(c => c.Id == categoryId);

            _logger.LogInformation($"Deleted category: {categoryId}");
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string categoryId)
        {
            var category = await _categoryRepository.FindOneAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(string categoryId, UpdateCategoryDto data)
        {
            var category = await _categoryRepository.FindOneAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            var oldCategoryName = category.Name;
            ImageFile? banner = data.Banner != null ? new ImageFile()
            {
                FileId = data.Banner.FileId,
                Url = data.Banner.Url,
            } : null;

            category.UpdateInfo(data.Name, banner);

            await _categoryRepository.UpdateAsync(category);

            if (data.Name != null && data.Name != oldCategoryName)
            {
                _courseRepository.UpdateCategoryInCourses(category);
            }

            _logger.LogInformation($"Category {category.Id} updated");

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
