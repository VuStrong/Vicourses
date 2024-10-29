namespace CourseService.Domain.Events.Enrollment
{
    public class UserEnrolledDomainEvent : DomainEvent
    {
        public string UserId { get; set; }
        public Models.Course Course { get; set; }

        public UserEnrolledDomainEvent(string userId, Models.Course course)
        {
            UserId = userId;
            Course = course;
        }
    }
}