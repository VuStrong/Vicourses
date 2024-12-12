using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RatingService.API.Application.Dtos;
using RatingService.API.Application.Dtos.Rating;
using RatingService.API.Application.Services;
using RatingService.API.Requests;
using System.Security.Claims;

namespace RatingService.API.Controllers
{
    [Route("api/rs/v1/ratings")]
    [Tags("Rating")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IAuthorizationService _authorizationService;

        public RatingsController(IRatingService ratingService, IAuthorizationService authorizationService)
        {
            _ratingService = ratingService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get ratings of a course
        /// </summary>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<RatingDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetRatingsByCourseRequest request, CancellationToken cancellationToken = default)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.CourseId, "GetRatingsByCourseIdPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var paramsDto = new GetRatingsParamsDto
            {
                Skip = request.Skip,
                Limit = request.Limit,
                CourseId = request.CourseId,
                Star = request.Star,
                Responded = request.Responded,
            };

            var result = await _ratingService.GetRatingsByCourseAsync(paramsDto, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get a rating of an user to a course
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Notfound</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserRating([FromQuery] string courseId, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var rating = await _ratingService.GetUserRatingAsync(userId, courseId, cancellationToken);

            return Ok(rating);
        }

        /// <summary>
        /// Create a rating
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Notfound</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(RatingDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CreateRatingRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var dto = new CreateRatingDto(request.CourseId, userId, request.Feedback, request.Star);
            var result = await _ratingService.CreateRatingAsync(dto);

            return CreatedAtAction(nameof(Get), result);
        }

        /// <summary>
        /// Update a rating
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Notfound</response>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(string id, UpdateRatingRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var dto = new UpdateRatingDto
            {
                Feedback = request.Feedback,
                Star = request.Star,
                UserId = userId,
            };
            var result = await _ratingService.UpdateRatingAsync(id, dto);

            return Ok(result);
        }

        /// <summary>
        /// Delete a rating
        /// </summary>
        /// <response code="200">Deleted</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Notfound</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _ratingService.DeleteRatingAsync(id, userId);

            return Ok();
        }

        /// <summary>
        /// Respond a rating
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Notfound</response>
        [HttpPost("{id}/response")]
        [Authorize]
        [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Respond(string id, RespondRatingRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var result = await _ratingService.RespondRatingAsync(id, new RespondRatingDto(userId, request.Response));

            return Ok(result);
        }
    }
}
