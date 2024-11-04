using System.ComponentModel.DataAnnotations;

namespace RatingService.API.Requests
{
    public class UpdateRatingRequest
    {
        [StringLength(400, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string? Feedback { get; set; }

        [Range(1, 5)]
        public int? Star { get; set; }
    }
}
