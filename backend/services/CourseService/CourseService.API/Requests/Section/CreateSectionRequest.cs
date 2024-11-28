using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Section
{
    public class CreateSectionRequest
    {
        [Required]
        [StringLength(80, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        public string CourseId { get; set; } = null!;

        [StringLength(200, ErrorMessage = "{0} length must not greater than {1}.")]
        public string? Description { get; set; }
    }
}
