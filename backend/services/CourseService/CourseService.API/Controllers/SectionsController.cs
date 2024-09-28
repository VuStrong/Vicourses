using CourseService.API.Models.Section;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Section;
using CourseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/sections")]
    [Tags("Course Section")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ICourseCurriculumService _courseCurriculumService;
        private readonly IAuthorizationService _authorizationService;

        public SectionsController(
            ICourseCurriculumService courseCurriculumService, 
            IAuthorizationService authorizationService)
        {
            _courseCurriculumService = courseCurriculumService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get one course section by ID
        /// </summary>
        /// <response code="403">Not allowed to get this section</response>
        /// <response code="404">Section not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionById(string id)
        {
            var section = await _courseCurriculumService.GetSectionByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, section, "GetSectionPolicy");

            if (authorizationResult.Succeeded)
            {
                return Ok(section);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Create a section for course.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the course can create</response>
        /// <response code="404">Course not found</response>
        [HttpPost]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSection(CreateSectionRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var section = await _courseCurriculumService.CreateSectionAsync(
                new CreateSectionDto(request.Title, request.CourseId, userId, request.Description)
            );

            return CreatedAtAction(
                nameof(GetSectionById),
                new { id = section.Id },
                section);
        }

        /// <summary>
        /// Update a section.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the section can update</response>
        /// <response code="404">Section not found</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(SectionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSection(string id, UpdateSectionRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var section = await _courseCurriculumService.UpdateSectionAsync(
                id,
                new UpdateSectionDto
                {
                    Title = request.Title,
                    Description = request.Description,
                },
                userId
            );

            return Ok(section);
        }

        /// <summary>
        /// Delete a section.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the section can delete</response>
        /// <response code="404">Section not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSection(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _courseCurriculumService.DeleteSectionAsync(id, userId);

            return Ok();
        }
    }
}
