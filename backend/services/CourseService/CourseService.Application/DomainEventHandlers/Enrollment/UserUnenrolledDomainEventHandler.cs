using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Enrollment;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Enrollment
{
    internal class UserUnenrolledDomainEventHandler : IDomainEventHandler<UserUnenrolledDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UserUnenrolledDomainEventHandler(IEventBus eventBus, IMapper mapper)
        {
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public Task Handle(UserUnenrolledDomainEvent @event)
        {
            var couseDto = _mapper.Map<CourseDto>(@event.Course);

            _eventBus.Publish(new UserUnenrolledIntegrationEvent(@event.UserId, couseDto, DateOnly.FromDateTime(DateTime.Now)));

            return Task.CompletedTask;
        }
    }
}
