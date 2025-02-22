namespace RatingService.API.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string InstructorId { get; set; }
        public string Title { get; set; }
        public string? ThumbnailUrl { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Published;
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }

        public Course(string id, string instructorId, string title)
        {
            Id = id;
            InstructorId = instructorId;
            Title = title;
        }
    }

    public enum CourseStatus
    {
        Published,
        Unpublished
    }
}
