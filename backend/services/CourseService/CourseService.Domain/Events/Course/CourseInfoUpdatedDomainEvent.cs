namespace CourseService.Domain.Events.Course
{
    public class CourseInfoUpdatedDomainEvent : DomainEvent
    {
        public Models.Course UpdatedCourse { get; set; }

        public CourseInfoUpdatedDomainEvent(Models.Course updatedCourse)
        {
            UpdatedCourse = updatedCourse;
        }
    }
}
