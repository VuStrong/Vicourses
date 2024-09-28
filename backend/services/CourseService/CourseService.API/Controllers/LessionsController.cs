using CourseService.API.Models.Lession;
using CourseService.API.Models.Quiz;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Lession;
using CourseService.Application.Dtos.Quiz;
using CourseService.Application.Interfaces;
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
        private readonly ILessionQuizService _lessionQuizService;
        private readonly IAuthorizationService _authorizationService;

        public LessionsController(
            ICourseCurriculumService courseCurriculumService,
            ILessionQuizService lessionQuizService,
            IAuthorizationService authorizationService)
        {
            _courseCurriculumService = courseCurriculumService;
            _lessionQuizService = lessionQuizService;
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

        /// <summary>
        /// Get all quizzes of a lession.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Lession not found</response>
        [HttpGet("{id}/quizzes")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuizDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuizzesByLessionId(string id)
        {
            var lession = await _courseCurriculumService.GetLessionByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, lession, "GetLessionPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var quizzes = await _lessionQuizService.GetQuizzesByLessionIdAsync(id);

            return Ok(quizzes);
        }

        /// <summary>
        /// Add a quiz for lession.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lession can add quiz</response>
        /// <response code="404">Lession not found</response>
        [HttpPost("{id}/quizzes")]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(QuizDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddLessionQuiz(string id, CreateLessionQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var quiz = await _lessionQuizService.CreateLessionQuizAsync(
                request.ToCreateLessionQuizDto(id, userId)
            );

            return CreatedAtAction(
                nameof(GetQuizzesByLessionId),
                new { id = id },
                quiz
            );
        }

        /// <summary>
        /// Update a quiz of a lession.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the quiz can update</response>
        /// <response code="404">Quiz not found</response>
        [HttpPatch("{id}/quizzes/{quizId}")]
        [Authorize]
        [ProducesResponseType(typeof(QuizDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLessionQuiz(string id, string quizId, UpdateLessionQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var quiz = await _lessionQuizService.UpdateLessionQuizAsync(
                quizId,
                request.ToUpdateLessionQuizDto(),
                userId
            );

            return Ok(quiz);
        }

        /// <summary>
        /// Delete a quiz of a lession.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the quiz can delete</response>
        /// <response code="404">Quiz not found</response>
        [HttpDelete("{id}/quizzes/{quizId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLessionQuiz(string id, string quizId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _lessionQuizService.DeleteLessionQuizAsync(quizId, userId);

            return Ok();
        }

        /// <summary>
        /// Change order of quizzes in a lession
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lession can update</response>
        /// <response code="404">Lession not found</response>
        [HttpPatch("{id}/quizzes/order")]
        [Authorize]
        public async Task<IActionResult> ChangeQuizzesOrder(string id, ChangeQuizzesOrderRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _lessionQuizService.ChangeOrderOfQuizzesAsync(id, request.QuizIds, userId);

            return Ok();
        }
    }
}
