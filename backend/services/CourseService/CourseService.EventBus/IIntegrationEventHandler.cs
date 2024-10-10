namespace CourseService.EventBus
{
    public interface IIntegrationEventHandler<T> : IIntegrationEventHandler where T : IntegrationEvent
    {
        Task Handle(T @event);

        Task IIntegrationEventHandler.Handle(IntegrationEvent @event) => Handle((T)@event);
    }

    public interface IIntegrationEventHandler
    {
        Task Handle(IntegrationEvent @event);
    }
}
