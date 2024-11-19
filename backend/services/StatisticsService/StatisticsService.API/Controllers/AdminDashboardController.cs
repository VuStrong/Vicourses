using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.API.Application.Dtos;
using StatisticsService.API.Application.Services;
using StatisticsService.API.Utils;

namespace StatisticsService.API.Controllers
{
    [Route("api/stats/v1/admin/dashboard")]
    [Tags("Admin dashboard")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardDataStatistician _statistician;

        public AdminDashboardController(IAdminDashboardDataStatistician statistician)
        {
            _statistician = statistician;
        }

        /// <summary>
        /// Get Admin dashboard
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(AdminDashboardDataDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminDashboard(CancellationToken cancellationToken = default)
        {
            var result = await _statistician.GetAdminDashboardDataAsync(cancellationToken);

            return Ok(result);
        }
    }
}
