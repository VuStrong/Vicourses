namespace RatingService.API.Application.Dtos.Rating
{
    public class GetRatingsByInstructorParamsDto
    {
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 15;
        public required string InstructorId { get; set; }
        public string? CourseId { get; set; }
        public int? Star { get; set; }
        public bool? Responded { get; set; }
    }
}