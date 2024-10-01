namespace CourseService.Domain.Events
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(DomainEvent domainEvent);

        Task DispatchFrom(Entity entity);
    }
}
