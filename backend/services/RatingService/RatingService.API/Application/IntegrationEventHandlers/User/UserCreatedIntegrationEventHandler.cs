using EventBus;
using RatingService.API.Application.IntegrationEvents.User;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.User
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(
            RatingServiceDbContext context, 
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle UserCreatedIntegrationEvent: {@event.Id}");

            var user = new Models.User(@event.Id, @event.Name, @event.Email, null);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
