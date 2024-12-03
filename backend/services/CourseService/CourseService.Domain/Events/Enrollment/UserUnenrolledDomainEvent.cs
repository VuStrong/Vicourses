namespace CourseService.Domain.Events.Enrollment
{
    public class UserUnenrolledDomainEvent : DomainEvent
    {
        public string UserId { get; set; }
        public Models.Course Course { get; set; }

        public UserUnenrolledDomainEvent(string userId, Models.Course course)
        {
            UserId = userId;
            Course = course;
        }
    }
}
