using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Lession
{
    public class UpdateLessionRequest
    {
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }
    }
}
