namespace CourseService.Domain.Events
{
    public interface IDomainEventHandler<T> : IDomainEventHandler where T : DomainEvent
    {
        Task Handle(T @event);

        Task IDomainEventHandler.Handle(DomainEvent @event) => Handle((T)@event);
    }

    public interface IDomainEventHandler
    {
        Task Handle(DomainEvent @event);
    }
}
