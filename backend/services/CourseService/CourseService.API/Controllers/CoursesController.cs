using CourseService.API.Models;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Course;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [Route("api/v1/courses")]
    [Tags("Course")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Get list of courses
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CourseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _courseService.GetCoursesAsync());
        }

        /// <summary>
        /// Get one course by ID
        /// </summary>
        /// <response code="403">Not allowed to get this course</response>
        /// <response code="404">Course not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseById(string id)
        {
            var course = await _courseService.GetCourseDetailByIdAsync(id);
            
            return Ok(course);
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
                new CreateCourseDto(request.Title, request.CategoryId, userId, request.Description)    
            );

            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = course.Id },
                course);
        }
    }
}
