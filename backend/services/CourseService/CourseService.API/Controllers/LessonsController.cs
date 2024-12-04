using CourseService.API.Requests.Comment;
using CourseService.API.Requests.Lesson;
using CourseService.API.Requests.Quiz;
using CourseService.API.Utils;
using CourseService.Application.Dtos.Comment;
using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Interfaces;
using CourseService.Shared.Paging;
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
        private readonly ICommentService _commentService;
        private readonly IAuthorizationService _authorizationService;

        public LessonsController(
            ICourseCurriculumService courseCurriculumService,
            ICommentService commentService,
            IAuthorizationService authorizationService)
        {
            _courseCurriculumService = courseCurriculumService;
            _commentService = commentService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get one course lesson by ID
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Not allowed to get this lesson</response>
        /// <response code="404">lesson not found</response>
        [HttpGet("{id}")]
        [Authorize]
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
        /// Add a quiz to a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can add quiz</response>
        /// <response code="404">lesson not found</response>
        [HttpPost("{id}/quizzes")]
        [Authorize(Roles = Roles.Instructor)]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddLessonQuiz(string id, CreateLessonQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.AddQuizToLessonAsync(
                id,
                request.ToCreateLessonQuizDto(userId)
            );

            return CreatedAtAction(
                nameof(GetLessonById),
                new { id = id },
                lesson
            );
        }

        /// <summary>
        /// Update a quiz in a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can update</response>
        /// <response code="404">Lesson not found</response>
        [HttpPatch("{id}/quizzes/{number}")]
        [Authorize]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLessonQuiz(string id, int number, UpdateLessonQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.UpdateQuizInLessonAsync(
                id,
                request.ToUpdateLessonQuizDto(userId, number)
            );

            return Ok(lesson);
        }

        /// <summary>
        /// Remove a quiz from a lesson.
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can delete</response>
        /// <response code="404">Lesson not found</response>
        [HttpDelete("{id}/quizzes/{number}")]
        [Authorize]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLessonQuiz(string id, int number)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.RemoveQuizFromLessonAsync(id, number, userId);

            return Ok(lesson);
        }

        /// <summary>
        /// Move a quizze in a lesson to another position
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Only owner of the lesson can update</response>
        /// <response code="404">Lesson not found</response>
        [HttpPost("{id}/quizzes/move")]
        [Authorize]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> MoveQuiz(string id, MoveQuizRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var lesson = await _courseCurriculumService.MoveQuizInLessonAsync(id, request.Number, request.To, userId);

            return Ok(lesson);
        }

        /// <summary>
        /// Get comments of a lesson
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">lesson not found</response>
        [HttpGet("{id}/comments")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<CommentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(string id, [FromQuery] GetCommentsRequest request, CancellationToken cancellationToken = default)
        {
            var lesson = await _courseCurriculumService.GetLessonByIdAsync(id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, lesson, "GetLessonPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var results = await _commentService.GetCommentsAsync(
                request.ToGetCommentsParamsDto(lesson.Id),
                cancellationToken
            );

            return Ok(results);
        }

        /// <summary>
        /// Create a comment
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">not found</response>
        [HttpPost("{id}/comments")]
        [Authorize]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateComment(string id, CreateCommentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var comment = await _commentService.CreateCommentAsync(
                new CreateCommentDto(id, userId, request.Content, request.ReplyToId));

            return CreatedAtAction(
                nameof(GetComments),
                new { id = id },
                comment
            );
        }

        /// <summary>
        /// Upvote a comment
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">not found</response>
        [HttpPost("{id}/comments/{commentId}/upvote")]
        [Authorize]
        public async Task<IActionResult> UpvoteComment(string id, string commentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _commentService.UpvoteCommentAsync(commentId, userId);

            return Ok();
        }

        /// <summary>
        /// Cancel upvote a comment
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">not found</response>
        [HttpPost("{id}/comments/{commentId}/cancel-upvote")]
        [Authorize]
        public async Task<IActionResult> CancelUpvoteComment(string id, string commentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _commentService.CancelUpvoteAsync(commentId, userId);

            return Ok();
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">not found</response>
        [HttpDelete("{id}/comments/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string id, string commentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _commentService.DeleteCommentAsync(commentId, userId);

            return Ok();
        }
    }
}
