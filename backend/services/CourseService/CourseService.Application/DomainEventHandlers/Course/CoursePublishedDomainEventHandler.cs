using AutoMapper;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.EventBus.Abstracts;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CoursePublishedDomainEventHandler : IDomainEventHandler<CoursePublishedDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public CoursePublishedDomainEventHandler(
            IEventBus eventBus,
            IMapper mapper)
        {
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public Task Handle(CoursePublishedDomainEvent @event)
        {
            var coursePublishedIntegrationEvent = _mapper.Map<CoursePublishedIntegrationEvent>(@event.Course);

            _eventBus.Publish(coursePublishedIntegrationEvent);

            return Task.CompletedTask;
        }
    }
}
