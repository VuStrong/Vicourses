namespace CourseService.Domain.Events.Section
{
    public class SectionDeletedDomainEvent : DomainEvent
    {
        public Models.Section Section { get; set; }

        public SectionDeletedDomainEvent(Models.Section section)
        {
            Section = section;
        }
    }
}
