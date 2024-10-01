namespace CourseService.Domain.Events
{
    public interface IDomainEventHandler<T> where T : DomainEvent
    {
        Task Handle(T @event);
    }
}
