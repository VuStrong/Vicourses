namespace CourseService.Domain.Events.Lession
{
    public class LessionDeletedDomainEvent : DomainEvent
    {
        public Models.Lession Lession { get; set; }

        public LessionDeletedDomainEvent(Models.Lession lession)
        {
            Lession = lession;
        }
    }
}
