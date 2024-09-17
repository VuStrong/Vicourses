using CourseService.API.Models.Course;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Services;
using CourseService.Shared.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/courses")]
    [Tags("Course")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IAuthorizationService _authorizationService;

        public CoursesController(ICourseService courseService, IAuthorizationService authorizationService)
        {
            _courseService = courseService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get courses
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CourseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourses(GetCoursesRequest request)
        {
            var results = await _courseService.GetCoursesAsync(request.ToGetCoursesDto());

            return Ok(results);
        }

        /// <summary>
        /// Get one course by ID
        /// </summary>
        /// <response code="403">Not allowed to get this course</response>
        /// <response code="404">Course not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseById(string id)
        {
            var course = await _courseService.GetCourseDetailByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, course, "GetCoursePolicy");
            
            if (authorizationResult.Succeeded)
            {
                return Ok(course);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Create a course.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only instructor role can create</response>
        /// <response code="404">Category or user not found</response>
        [HttpPost]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var course = await _courseService.CreateCourseAsync(
                new CreateCourseDto(request.Title, request.CategoryId, request.SubCategoryId, 
                    userId, request.Description)    
            );

            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = course.Id },
                course);
        }

        /// <summary>
        /// Update a course.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Course not found</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, UpdateCourseRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var course = await _courseService.UpdateCourseAsync(
                id,
                request.ToUpdateCourseDto(),
                userId
            );

            return Ok(course);
        }

        /// <summary>
        /// Delete a course.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Course cannot be deleted (have students enrolled)</response>
        /// <response code="404">Course not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _courseService.DeleteCourseAsync(id, userId);

            return Ok();
        }
    }
}
