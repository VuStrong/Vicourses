namespace CourseService.Domain.Events.Section
{
    public class SectionCreatedDomainEvent : DomainEvent
    {
        public Models.Section Section { get; set; }

        public SectionCreatedDomainEvent(Models.Section section)
        {
            Section = section;
        }
    }
}
