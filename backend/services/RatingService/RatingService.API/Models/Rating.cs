namespace RatingService.API.Models
{
    public class Rating
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public string Feedback { get; set; }
        public int Star { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Responded { get; set; }
        public string? Response { get; set; }
        public DateTime? RespondedAt { get; set; }

        public Rating(string courseId, string userId, string feedback, int star)
        {
            Id = Guid.NewGuid().ToString();
            CourseId = courseId;
            UserId = userId;
            Feedback = feedback;
            Star = star;
            CreatedAt = DateTime.Now;
        }
    }
}
