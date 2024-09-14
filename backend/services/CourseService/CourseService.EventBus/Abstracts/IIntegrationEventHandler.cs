using CourseService.EventBus.Events;

namespace CourseService.EventBus.Abstracts
{
    public interface IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        Task Handle(T @event);
    }
}
