using CourseService.Application.IntegrationEvents.Email;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseApprovedDomainEventHandler : IDomainEventHandler<CourseApprovedDomainEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;

        public CourseApprovedDomainEventHandler(IUserRepository userRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(CourseApprovedDomainEvent @event)
        {
            var instructor = await _userRepository.FindOneAsync(@event.Course.User.Id);

            if (instructor == null) return;

            _eventBus.Publish(new SendEmailIntegrationEvent
            {
                To = instructor.Email,
                Template = "course_approved",
                Payload = new 
                {
                    username = instructor.Name,
                    courseId = @event.Course.Id,
                    courseName = @event.Course.Title
                }
            });
        }
    }
}
