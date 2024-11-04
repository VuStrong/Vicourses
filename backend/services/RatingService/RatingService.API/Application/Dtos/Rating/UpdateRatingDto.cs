namespace RatingService.API.Application.Dtos.Rating
{
    public class UpdateRatingDto
    {
        public string? Feedback { get; set; }
        public int? Star { get; set; }
        public required string UserId { get; set; }
    }
}
