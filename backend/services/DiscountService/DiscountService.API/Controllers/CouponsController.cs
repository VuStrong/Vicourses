using DiscountService.API.Application;
using DiscountService.API.Application.Dtos;
using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Application.Exceptions;
using DiscountService.API.Application.Services;
using DiscountService.API.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscountService.API.Controllers
{
    [Route("api/ds/v1/[controller]")]
    [Tags("Coupon")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IAuthorizationService _authorizationService;

        public CouponsController(ICouponService couponService, IAuthorizationService authorizationService)
        {
            _couponService = couponService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get coupons of a course
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You do not have permission to get coupons of the course</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<CouponDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetCouponsByCourseRequest request, CancellationToken cancellationToken = default)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.CourseId, "GetCouponsByCoursePolicy");

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException($"You do not have permission to get coupons of the course '{request.CourseId}'");
            }

            var paramsDto = new GetCouponsByCourseParamsDto
            {
                CourseId = request.CourseId,
                Skip = request.Skip,
                Limit = request.Limit,
                IsExpired = request.IsExpired,
            };
            var result = await _couponService.GetCouponsByCourseAsync(paramsDto, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Create a coupon
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You do not have permission to create coupon for the course</response>
        /// <response code="404">Course not found</response>
        /// <response code="422">Failed business validation</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CouponDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CreateCouponRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var dto = new CreateCouponDto
            {
                CourseId = request.CourseId,
                UserId = userId,
                Days = request.Days,
                Availability = request.Availability,
                Discount = request.Discount,
            };
            var result = await _couponService.CreateCouponAsync(dto);

            return CreatedAtAction(nameof(Get), result);
        }

        /// <summary>
        /// Update a coupon
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You do not have permission to perform operations on the coupon</response>
        /// <response code="404">Coupon not found</response>
        [HttpPatch("{code}")]
        [Authorize]
        public async Task<IActionResult> Patch(string code, SetCouponActiveRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var dto = new SetCouponActiveDto(code, userId, request.IsActive);
            await _couponService.SetCouponActiveAsync(dto);

            return Ok();
        }

        /// <summary>
        /// Check a coupon valid or not
        /// </summary>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{code}/check")]
        [Authorize]
        [ProducesResponseType(typeof(CouponCheckResultDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Check(string code, [FromQuery] string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var result = await _couponService.CheckCouponAsync(new CheckCouponDto
            {
                Code = code,
                CourseId = courseId,
                UserId = userId
            });

            return Ok(result);
        }
    }
}
