using EventBus;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Infrastructure.Repositories;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class UserUnenrolledIntegrationEventHandler : IIntegrationEventHandler<UserUnenrolledIntegrationEvent>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ILogger<UserUnenrolledIntegrationEventHandler> _logger;

        public UserUnenrolledIntegrationEventHandler(
            IWishlistRepository wishlistRepository,
            ILogger<UserUnenrolledIntegrationEventHandler> logger)
        {
            _wishlistRepository = wishlistRepository;
            _logger = logger;
        }

        public async Task Handle(UserUnenrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle UserUnenrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var wishlist = await _wishlistRepository.FindByUserIdAsync(@event.UserId);

            if (wishlist != null)
            {
                wishlist.RemoveEnrollCourse(@event.Course.Id);

                await _wishlistRepository.UpdateWishlistAsync(wishlist);
            }
        }
    }
}
