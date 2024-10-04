using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CourseService.API.Controllers
{
    [Route("api/cs/v1/locales")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class LocalesController : ControllerBase
    {
        /// <summary>
        /// Get all locales
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<LocaleResponse>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var locales = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            var response = locales.Select(l => new LocaleResponse
            {
                Name = l.Name,
                EnglishTitle = l.EnglishName
            });

            return Ok(response);
        }
    }

    public class LocaleResponse
    {
        public string Name { get; set; } = string.Empty;
        public string EnglishTitle { get; set; } = string.Empty;
    }
}
