namespace PaymentService.API.Models
{
    public class Course
    {
        public string Id { get; private set; }
        public string Title { get; set; }
        public string CreatorId { get; private set; }
        public decimal Price { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Published;

        public Course(string id, string title, string creatorId, decimal price)
        {
            Id = id;
            Title = title;
            CreatorId = creatorId;
            Price = price;
        }
    }

    public enum CourseStatus
    {
        Published,
        Unpublished,
    }
}
