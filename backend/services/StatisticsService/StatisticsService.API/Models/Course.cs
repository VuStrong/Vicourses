namespace StatisticsService.API.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string InstructorId { get; set; }
        public DateOnly PublishedAt { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Published;

        public Course(string id, string instructorId)
        {
            Id = id;
            InstructorId = instructorId;
            PublishedAt = DateOnly.FromDateTime(DateTime.Today);
        }
    }

    public enum CourseStatus
    {
        Published,
        Unpublished
    }
}
