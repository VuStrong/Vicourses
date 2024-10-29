using CourseService.API.Models.Category;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Category;
using CourseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/categories")]
    [Tags("Category")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IDataAggregator _dataAggregator;

        public CategoriesController(ICategoryService categoryService, IDataAggregator dataAggregator)
        {
            _categoryService = categoryService;
            _dataAggregator = dataAggregator;
        }

        /// <summary>
        /// Get all available categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetCategoriesRequest query)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(query.ToGetCategoriesDto());

            return Ok(categories);
        }

        /// <summary>
        /// Get navigation categories
        /// </summary>
        [HttpGet("navigation-list")]
        [ProducesResponseType(typeof(List<CategoryWithSubsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNavigationCategories()
        {
            var results = await _dataAggregator.GetRootCategoriesWithSubCategoriesAsync();

            return Ok(results);
        }

        /// <summary>
        /// Get one category by slug
        /// </summary>
        /// <response code="404">Category not found</response>
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryBySlug(string slug)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);

            return Ok(category);
        }

        /// <summary>
        /// Create a category (ADMIN required).
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only admin can create</response>
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateCategoryAsync(
                new CreateCategoryDto(request.Name, request.ParentId)  
            );

            return CreatedAtAction(
                nameof(GetCategoryBySlug),
                new { slug = category.Slug },
                category);
        }

        /// <summary>
        /// Update a category (ADMIN required).
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only admin can update</response>
        /// <response code="404">Category not found</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCategory(string id, UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateCategoryAsync(
                id,
                request.ToUpdateCategoryDto()
            );

            return Ok(category);
        }

        /// <summary>
        /// Delete a category (ADMIN required).
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only admin can delete</response>
        /// <response code="404">Category not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return Ok();
        }
    }
}
