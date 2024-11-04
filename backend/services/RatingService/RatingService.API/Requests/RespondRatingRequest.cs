using System.ComponentModel.DataAnnotations;

namespace RatingService.API.Requests
{
    public class RespondRatingRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Response { get; set; } = null!;
    }
}
