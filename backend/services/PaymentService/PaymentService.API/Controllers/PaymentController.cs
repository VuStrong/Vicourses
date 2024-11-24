using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Exceptions;
using PaymentService.API.Application.Services;
using PaymentService.API.Requests;
using System.Security.Claims;

namespace PaymentService.API.Controllers
{
    [Route("api/ps/v1/[controller]")]
    [Tags("Payment")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get user payments
        /// </summary>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PaymentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetUserPaymentsRequest request, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var results = await _paymentService.GetUserPaymentsAsync(userId, request.Skip, request.Limit, cancellationToken);

            return Ok(results);
        }

        /// <summary>
        /// Get a payment by ID
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Payment not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<PaymentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var payment = await _paymentService.GetPaymentAsync(id, cancellationToken);

            if (userId != payment.UserId && userRole != "admin")
            {
                throw new AppException(
                    "You do not have permission to get this payment",
                    403);
            }

            return Ok(payment);
        }

        /// <summary>
        /// Create payment using Paypal gateway
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Business validation failed</response>
        [HttpPost("paypal")]
        [Authorize]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreatePaypalPayment(CreatePaypalPaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var payload = new CreatePaymentDto
            {
                UserId = userId,
                Username = request.Username,
                Email = request.Email,
                CourseId = request.CourseId,
                CourseName = request.CourseName,
                CouponCode = request.CouponCode,
            };

            var payment = await _paymentService.CreatePaypalPaymentAsync(payload);

            return Ok(payment);
        }

        /// <summary>
        /// Capture and complete the paypal payment
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Business validation failed</response>
        [HttpPost("paypal/{id}/capture")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Authorize]
        public async Task<IActionResult> CapturePaypalPayment(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _paymentService.CapturePaypalPaymentAsync(id, userId);

            return Ok();
        }

        /// <summary>
        /// Cancel payment
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Business validation failed</response>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Authorize]
        public async Task<IActionResult> CancelPayment(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _paymentService.CancelPaymentAsync(id, userId);

            return Ok();
        }
    }
}
