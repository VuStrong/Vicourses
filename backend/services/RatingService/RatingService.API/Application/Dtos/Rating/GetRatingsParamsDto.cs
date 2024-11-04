namespace RatingService.API.Application.Dtos.Rating
{
    public class GetRatingsParamsDto
    {
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 15;
        public required string CourseId { get; set; }
        public int? Star { get; set; }
        public bool? Responded { get; set; }
    }
}
