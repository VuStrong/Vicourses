namespace RatingService.API.Application.Dtos.Rating
{
    public class RatingDto
    {
        public string Id { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public PublicCourseDto? Course { get; set; }
        public string UserId { get; set; } = string.Empty;
        public PublicUserDto User { get; set; } = null!;
        public string Feedback { get; set; } = string.Empty;
        public int Star { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Responded { get; set; }
        public string? Response { get; set; }
        public DateTime? RespondedAt { get; set; }
    }
}
