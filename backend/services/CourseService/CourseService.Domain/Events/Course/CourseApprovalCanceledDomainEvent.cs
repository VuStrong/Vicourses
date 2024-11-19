namespace CourseService.Domain.Events.Course
{
    public class CourseApprovalCanceledDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }
        public List<string> Reasons { get; set; } = [];
        
        public CourseApprovalCanceledDomainEvent(Models.Course course, List<string> reasons)
        {
            Course = course;
            Reasons = reasons;
        }
    }
}
