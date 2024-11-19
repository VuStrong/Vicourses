using EventBus;
using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.IntegrationEvents.User;
using StatisticsService.API.Infrastructure;

namespace StatisticsService.API.Application.IntegrationEventHandlers.User
{
    public class UserRoleUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserRoleUpdatedIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<UserRoleUpdatedIntegrationEventHandler> _logger;

        public UserRoleUpdatedIntegrationEventHandler(
            StatisticsServiceDbContext dbContext,
            ILogger<UserRoleUpdatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserRoleUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle UserRoleUpdatedIntegrationEvent: {@event.Id}");

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);

            if (user == null)
            {
                user = new Models.User(@event.Id, @event.Role);
                _dbContext.Users.Add(user);
            }
            else
            {
                user.Role = @event.Role;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
