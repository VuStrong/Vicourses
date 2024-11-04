using System.ComponentModel.DataAnnotations;

namespace RatingService.API.Requests
{
    public class CreateRatingRequest
    {
        [Required]
        public string CourseId { get; set; } = null!;

        [Required]
        [StringLength(400, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Feedback { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Star { get; set; }
    }
}
