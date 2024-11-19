using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.API.Application.Dtos;
using StatisticsService.API.Application.Services;
using System.Security.Claims;

namespace StatisticsService.API.Controllers
{
    [Route("api/stats/v1/instructor/performance")]
    [Tags("Instructor performance")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class InstructorPerformanceController : ControllerBase
    {
        private readonly IInstructorPerformanceStatistician _statistician;

        public InstructorPerformanceController(IInstructorPerformanceStatistician statistician)
        {
            _statistician = statistician;
        }

        /// <summary>
        /// Get instructor performance metrics
        /// </summary>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(InstructorPerformanceDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstructorPerformance([FromQuery] DateScope scope = DateScope.Month, 
            [FromQuery] string? courseId = null, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var result = await _statistician.GetInstructorPerformanceAsync(userId, scope, courseId, cancellationToken);

            return Ok(result);
        }
    }
}
