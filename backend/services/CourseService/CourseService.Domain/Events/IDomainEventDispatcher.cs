namespace CourseService.Domain.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchFrom(Entity entity);
    }
}
