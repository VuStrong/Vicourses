using CourseService.Application.IntegrationEvents.User;
using CourseService.EventBus.Abstracts;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.User
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("CourseService handle UserCreated event {Id}", @event.Id);
        }
    }
}
