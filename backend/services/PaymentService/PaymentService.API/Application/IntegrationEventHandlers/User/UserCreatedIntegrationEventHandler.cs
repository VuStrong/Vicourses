using EventBus;
using PaymentService.API.Application.IntegrationEvents.User;
using PaymentService.API.Infrastructure;

namespace PaymentService.API.Application.IntegrationEventHandlers.User
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle UserCreatedIntegrationEvent: {msg}", @event.Id);

            var user = new Models.User(@event.Id, @event.Name, @event.Email);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
