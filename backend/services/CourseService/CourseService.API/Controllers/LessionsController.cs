using CourseService.API.Models.Lession;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/lessions")]
    [Tags("Course Lession")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class LessionsController : ControllerBase
    {
        private readonly ICourseCurriculumService _courseCurriculumService;
        private readonly IAuthorizationService _authorizationService;

        public LessionsController(
            ICourseCurriculumService courseCurriculumService,
            IAuthorizationService authorizationService)
        {
            _courseCurriculumService = courseCurriculumService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get one course lession by ID
        /// </summary>
        /// <response code="403">Not allowed to get this lession</response>
        /// <response code="404">Lession not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLessionById(string id)
        {
            var lession = await _courseCurriculumService.GetLessionByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, lession, "GetLessionPolicy");

            if (authorizationResult.Succeeded)
            {
                return Ok(lession);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Create a lession for course.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the course can create</response>
        /// <response code="404">Course or section not found</response>
        [HttpPost]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(LessionDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateLession(CreateLessionRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lession = await _courseCurriculumService.CreateLessionAsync(
                new CreateLessionDto(request.Title, request.CourseId, request.SectionId, userId,
                    request.Type, request.Description)
            );

            return CreatedAtAction(
                nameof(GetLessionById),
                new { id = lession.Id },
                lession);
        }

        /// <summary>
        /// Update a lession.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lession can update</response>
        /// <response code="404">Lession not found</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(LessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLession(string id, UpdateLessionRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lession = await _courseCurriculumService.UpdateLessionAsync(
                id,
                new UpdateLessionDto
                {
                    Title = request.Title,
                    Description = request.Description
                },
                userId
            );

            return Ok(lession);
        }

        /// <summary>
        /// Delete a lession.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lession can delete</response>
        /// <response code="404">Lession not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLession(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _courseCurriculumService.DeleteLessionAsync(id, userId);

            return Ok();
        }
    }
}
