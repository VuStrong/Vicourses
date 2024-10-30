using AutoMapper;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseInfoUpdatedDomainEventHandler : IDomainEventHandler<CourseInfoUpdatedDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public CourseInfoUpdatedDomainEventHandler(IEventBus eventBus, IMapper mapper)
        {
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public Task Handle(CourseInfoUpdatedDomainEvent @event)
        {
            if (@event.UpdatedCourse.Status == Domain.Enums.CourseStatus.Published)
            {
                var courseInfoUpdatedIntegrationEvent = _mapper.Map<CourseInfoUpdatedIntegrationEvent>(@event.UpdatedCourse);

                _eventBus.Publish(courseInfoUpdatedIntegrationEvent);
            }

            return Task.CompletedTask;
        }
    }
}
