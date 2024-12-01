using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.IntegrationEvents.User;
using PaymentService.API.Infrastructure;

namespace PaymentService.API.Application.IntegrationEventHandlers.User
{
    public class UserInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserInfoUpdatedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<UserInfoUpdatedIntegrationEventHandler> _logger;

        public UserInfoUpdatedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<UserInfoUpdatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle UserInfoUpdatedIntegrationEvent: {msg}", @event.Id);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);

            if (user == null)
            {
                user = new Models.User(@event.Id, @event.Name, @event.Email);
                _dbContext.Users.Add(user);
            }
            else
            {
                user.Name = @event.Name;
                user.Email = @event.Email;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
