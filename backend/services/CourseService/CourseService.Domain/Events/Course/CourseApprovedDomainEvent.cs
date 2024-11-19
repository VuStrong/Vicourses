namespace CourseService.Domain.Events.Course
{
    public class CourseApprovedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }

        public CourseApprovedDomainEvent(Models.Course course)
        {
            Course = course;
        }
    }
}
