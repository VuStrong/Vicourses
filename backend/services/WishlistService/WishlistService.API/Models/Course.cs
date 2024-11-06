namespace WishlistService.API.Models
{
    public record UserInCourse(string Id, string Name, string? ThumbnailUrl);

    public class Course
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TitleCleaned { get; set; }
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public string? ThumbnailUrl { get; set; }
        public UserInCourse User { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Published;

        public Course(string id, string title, string titleCleaned, UserInCourse user)
        {
            Id = id;
            Title = title;
            TitleCleaned = titleCleaned;
            User = user;
        }
    }

    public enum CourseStatus
    {
        Published,
        Unpublished
    }
}
