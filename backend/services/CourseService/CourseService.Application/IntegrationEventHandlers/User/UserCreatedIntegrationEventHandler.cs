using CourseService.Application.IntegrationEvents.User;
using CourseService.Domain.Contracts;
using CourseService.EventBus.Abstracts;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.User
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(
            IUserRepository userRepository,
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("CourseService handle UserCreated event {Id}", @event.Id);

            var user = Domain.Models.User.Create(@event.Id, @event.Name, @event.Email, null);

            await _userRepository.CreateAsync(user);
        }
    }
}
