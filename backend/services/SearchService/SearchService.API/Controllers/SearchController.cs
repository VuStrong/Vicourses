using Microsoft.AspNetCore.Mvc;
using SearchService.API.Application.Dtos;
using SearchService.API.Application.Interfaces;
using SearchService.API.Models;

namespace SearchService.API.Controllers
{
    [Route("api/ss/v1/search")]
    [Tags("Search")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ICoursesQueryService _searchService;

        public SearchController(ICoursesQueryService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Search for courses
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Course>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchCourses([FromQuery]SearchCoursesParams request, CancellationToken cancellationToken = default)
        {
            var result = await _searchService.SearchCoursesAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
