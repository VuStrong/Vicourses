namespace CourseService.Domain.Events.Course
{
    public class CourseDeletedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }

        public CourseDeletedDomainEvent(Models.Course course)
        {
            Course = course;
        }
    }
}
