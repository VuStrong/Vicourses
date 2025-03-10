﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.Services;

namespace WishlistService.API.Controllers
{
    [Route("api/ws/v1/[controller]")]
    [Tags("Wishlist")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        /// <summary>
        /// Get user's wishlist
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Wishlist not found</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(WishlistDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var result = await _wishlistService.GetUserWishlistAsync(userId, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Check if a course added to wishlist or not
        /// </summary>
        /// <response code="200">Added</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not added</response>
        [HttpHead("courses/{courseId}")]
        [Authorize]
        public async Task<IActionResult> CheckCourse(string courseId, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var added = await _wishlistService.CheckCourseInUserWishlistAsync(userId, courseId, cancellationToken);

            if (!added)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Add a course to wishlist
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Course not found</response>
        [HttpPost("courses/{courseId}")]
        [Authorize]
        [ProducesResponseType(typeof(WishlistDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var result = await _wishlistService.AddCourseToUserWishlistAsync(
                new AddToWishlistDto
                {
                    UserId = userId,
                    Email = userEmail,
                    CourseId = courseId,
                }    
            );

            return CreatedAtAction(nameof(Get), result);
        }

        /// <summary>
        /// Remove a course from wishlist
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Wishlist not found</response>
        [HttpDelete("courses/{courseId}")]
        [Authorize]
        [ProducesResponseType(typeof(WishlistDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var result = await _wishlistService.RemoveCourseFromUserWishlistAsync(userId, courseId);

            return Ok(result);
        }

        /// <summary>
        /// Get an array contains ID of courses added to wishlist
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("courses/ids")]
        [Authorize]
        public async Task<IActionResult> GetCourseIds(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var ids = await _wishlistService.GetCourseIdsInUserWishlistAsync(userId, cancellationToken);

            return Ok(new 
            {
                CourseIds = ids,
            });
        }
    }
}
