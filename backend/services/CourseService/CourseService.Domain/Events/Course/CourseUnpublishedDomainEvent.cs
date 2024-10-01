namespace CourseService.Domain.Events.Course
{
    public class CourseUnpublishedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }

        public CourseUnpublishedDomainEvent(Models.Course course)
        {
            Course = course;
        }
    }
}
