using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseUnpublishedDomainEventHandler : IDomainEventHandler<CourseUnpublishedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public CourseUnpublishedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(CourseUnpublishedDomainEvent @event)
        {
            var courseUnpublishedIntegrationEvent = new CourseUnpublishedIntegrationEvent()
            {
                Id = @event.Course.Id
            };

            _eventBus.Publish(courseUnpublishedIntegrationEvent);

            return Task.CompletedTask;
        }
    }
}
