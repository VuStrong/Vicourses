namespace RatingService.API.Models
{
    public class Enrollment
    {
        public string CourseId { get; set; }
        public string UserId { get; set; }

        public Enrollment(string courseId, string userId)
        {
            CourseId = courseId;
            UserId = userId;
        }
    }
}
