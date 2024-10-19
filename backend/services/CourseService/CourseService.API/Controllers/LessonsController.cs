using CourseService.API.Models.Lesson;
using CourseService.API.Models.Quiz;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Dtos.Quiz;
using CourseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/lessons")]
    [Tags("Course Lesson")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ICourseCurriculumService _courseCurriculumService;
        private readonly ILessonQuizService _lessonQuizService;
        private readonly IAuthorizationService _authorizationService;

        public LessonsController(
            ICourseCurriculumService courseCurriculumService,
            ILessonQuizService lessonQuizService,
            IAuthorizationService authorizationService)
        {
            _courseCurriculumService = courseCurriculumService;
            _lessonQuizService = lessonQuizService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get one course lesson by ID
        /// </summary>
        /// <response code="403">Not allowed to get this lesson</response>
        /// <response code="404">lesson not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLessonById(string id)
        {
            var lesson = await _courseCurriculumService.GetLessonByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, lesson, "GetLessonPolicy");

            if (authorizationResult.Succeeded)
            {
                return Ok(lesson);
            }
            else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Create a lesson for course.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the course can create</response>
        /// <response code="404">Course or section not found</response>
        [HttpPost]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateLesson(CreateLessonRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.CreateLessonAsync(
                new CreateLessonDto(request.Title, request.CourseId, request.SectionId, userId,
                    request.Type, request.Description)
            );

            return CreatedAtAction(
                nameof(GetLessonById),
                new { id = lesson.Id },
                lesson);
        }

        /// <summary>
        /// Update a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can update</response>
        /// <response code="404">lesson not found</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLesson(string id, UpdateLessonRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.UpdateLessonAsync(
                id,
                request.ToUpdateLessonDto(),
                userId
            );

            return Ok(lesson);
        }

        /// <summary>
        /// Delete a lesson.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can delete</response>
        /// <response code="404">lesson not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLesson(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _courseCurriculumService.DeleteLessonAsync(id, userId);

            return Ok();
        }

        /// <summary>
        /// Get all quizzes of a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">lesson not found</response>
        [HttpGet("{id}/quizzes")]
        [Authorize]
        [ProducesResponseType(typeof(List<QuizDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuizzesByLessonId(string id)
        {
            var lesson = await _courseCurriculumService.GetLessonByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, lesson, "GetLessonPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var quizzes = await _lessonQuizService.GetQuizzesByLessonIdAsync(id);

            return Ok(quizzes);
        }

        /// <summary>
        /// Add a quiz for lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can add quiz</response>
        /// <response code="404">lesson not found</response>
        [HttpPost("{id}/quizzes")]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(QuizDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddLessonQuiz(string id, CreateLessonQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var quiz = await _lessonQuizService.CreateLessonQuizAsync(
                request.ToCreateLessonQuizDto(id, userId)
            );

            return CreatedAtAction(
                nameof(GetQuizzesByLessonId),
                new { id = id },
                quiz
            );
        }

        /// <summary>
        /// Update a quiz of a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the quiz can update</response>
        /// <response code="404">Quiz not found</response>
        [HttpPatch("{id}/quizzes/{quizId}")]
        [Authorize]
        [ProducesResponseType(typeof(QuizDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLessonQuiz(string id, string quizId, UpdateLessonQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var quiz = await _lessonQuizService.UpdateLessonQuizAsync(
                quizId,
                request.ToUpdateLessonQuizDto(),
                userId
            );

            return Ok(quiz);
        }

        /// <summary>
        /// Delete a quiz of a lesson.
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the quiz can delete</response>
        /// <response code="404">Quiz not found</response>
        [HttpDelete("{id}/quizzes/{quizId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLessonQuiz(string id, string quizId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _lessonQuizService.DeleteLessonQuizAsync(quizId, userId);

            return Ok();
        }

        /// <summary>
        /// Change order of quizzes in a lesson
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can update</response>
        /// <response code="404">Lesson not found</response>
        [HttpPatch("{id}/quizzes/order")]
        [Authorize]
        public async Task<IActionResult> ChangeQuizzesOrder(string id, ChangeQuizzesOrderRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _lessonQuizService.ChangeOrderOfQuizzesAsync(id, request.QuizIds, userId);

            return Ok();
        }
    }
}
