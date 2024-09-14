using CourseService.EventBus.Events;

namespace CourseService.EventBus.Abstracts
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : IntegrationEvent;

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
