namespace RatingService.API.Application.Dtos
{
    public class PublicCourseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
    }
}
