namespace CourseService.Domain.Events.Course
{
    public class CoursePublishedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }

        public CoursePublishedDomainEvent(Models.Course course)
        {
            Course = course;
        }
    }
}
