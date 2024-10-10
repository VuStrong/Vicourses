namespace CourseService.EventBus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : IntegrationEvent;
    }
}
