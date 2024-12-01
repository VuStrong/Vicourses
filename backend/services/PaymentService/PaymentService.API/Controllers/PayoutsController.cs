using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Services;
using PaymentService.API.Application.Services.Paypal;
using PaymentService.API.Models;
using PaymentService.API.Requests;

namespace PaymentService.API.Controllers
{
    [Route("api/ps/v1/[controller]")]
    [Tags("Payout")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class PayoutsController : ControllerBase
    {
        private readonly IPayoutService _payoutService;
        private readonly IPaypalPayoutsService _paypalPayoutsService;

        public PayoutsController(IPayoutService payoutService, IPaypalPayoutsService paypalPayoutsService)
        {
            _payoutService = payoutService;
            _paypalPayoutsService = paypalPayoutsService;
        }

        /// <summary>
        /// Get batch payouts (ADMIN required)
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(PagedResult<BatchPayout>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetBatchPayoutsRequest query, CancellationToken cancellationToken = default)
        {
            var result = await _payoutService.GetPayoutsAsync(query.Skip, query.Limit, query.From, query.To, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get one batch payout (ADMIN required)
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BatchPayout), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _payoutService.GetPayoutAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get paypal batch payout details
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("paypal-batch-payout/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPaypalBatchPayout(string id)
        {
            var result = await _paypalPayoutsService.GetBatchPayoutAsync(id);
            return Ok(result);
        }
    }
}
