using AutoMapper;
using CourseService.Application.Dtos.Course;
using CourseService.Application.IntegrationEvents.Course;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Enrollment;
using CourseService.EventBus;

namespace CourseService.Application.DomainEventHandlers.Enrollment
{
    public class UserEnrolledDomainEventHandler : IDomainEventHandler<UserEnrolledDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UserEnrolledDomainEventHandler(IEventBus eventBus, IMapper mapper)
        {
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public Task Handle(UserEnrolledDomainEvent @event)
        {
            var couseDto = _mapper.Map<CourseDto>(@event.Course);
            
            _eventBus.Publish(new UserEnrolledIntegrationEvent(@event.UserId, couseDto)
            {
                EnrolledAt = DateOnly.FromDateTime(DateTime.Now),
            });

            return Task.CompletedTask;
        }
    }
}