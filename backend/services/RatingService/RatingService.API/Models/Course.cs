namespace RatingService.API.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string InstructorId { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Published;
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }

        public Course(string id, string instructorId)
        {
            Id = id;
            InstructorId = instructorId;
        }
    }

    public enum CourseStatus
    {
        Published,
        Unpublished
    }
}
