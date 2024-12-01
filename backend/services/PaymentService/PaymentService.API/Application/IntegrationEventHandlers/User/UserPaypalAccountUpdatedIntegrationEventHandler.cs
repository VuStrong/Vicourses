using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.IntegrationEvents.User;
using PaymentService.API.Infrastructure;

namespace PaymentService.API.Application.IntegrationEventHandlers.User
{
    public class UserPaypalAccountUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserPaypalAccountUpdatedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<UserPaypalAccountUpdatedIntegrationEventHandler> _logger;

        public UserPaypalAccountUpdatedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<UserPaypalAccountUpdatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserPaypalAccountUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle UserPaypalAccountUpdatedIntegrationEvent: {msg}", @event.Id);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);

            if (user != null)
            {
                user.PaypalPayerId = @event.PaypalAccount.PayerId;
                user.PaypalEmail = @event.PaypalAccount.Email;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
