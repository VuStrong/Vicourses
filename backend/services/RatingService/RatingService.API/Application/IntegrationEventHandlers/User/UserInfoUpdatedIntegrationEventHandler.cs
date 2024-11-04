using EventBus;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Application.IntegrationEvents.User;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.User
{
    public class UserInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserInfoUpdatedIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<UserInfoUpdatedIntegrationEventHandler> _logger;

        public UserInfoUpdatedIntegrationEventHandler(
            RatingServiceDbContext context,
            ILogger<UserInfoUpdatedIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(UserInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle UserInfoUpdatedIntegrationEvent: {@event.Id}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == @event.Id);

            if (user != null)
            {
                await UpdateUser(user, @event);
            }
            else
            {
                await AddUser(@event);
            }
        }

        private async Task AddUser(UserInfoUpdatedIntegrationEvent @event)
        {
            var user = new Models.User(@event.Id, @event.Name, @event.Email, @event.ThumbnailUrl);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        private async Task UpdateUser(Models.User user, UserInfoUpdatedIntegrationEvent @event)
        {
            user.Name = @event.Name;
            user.ThumbnailUrl = @event.ThumbnailUrl;

            await _context.SaveChangesAsync();
        }
    }
}
