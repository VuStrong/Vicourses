using EventBus;
using StatisticsService.API.Application.IntegrationEvents.User;
using StatisticsService.API.Infrastructure;
using StatisticsService.API.Utils;

namespace StatisticsService.API.Application.IntegrationEventHandlers.User
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(
            StatisticsServiceDbContext dbContext, 
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle UserCreatedIntegrationEvent: {@event.Id}");

            var user = new Models.User(@event.Id, Roles.Student);
            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
        }
    }
}
