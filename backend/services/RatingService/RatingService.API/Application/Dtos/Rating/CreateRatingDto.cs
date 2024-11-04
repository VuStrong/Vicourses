namespace RatingService.API.Application.Dtos.Rating
{
    public class CreateRatingDto
    {
        public string CourseId { get; set; }
        public string UserId { get; set; }
        public string Feedback { get; set; }
        public int Star { get; set; }

        public CreateRatingDto(string courseId, string userId, string feedback, int star)
        {
            CourseId = courseId;
            UserId = userId;
            Feedback = feedback;
            Star = star;
        }
    }
}
