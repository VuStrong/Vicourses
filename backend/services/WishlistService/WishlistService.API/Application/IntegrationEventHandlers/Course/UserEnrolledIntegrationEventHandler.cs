using EventBus;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Infrastructure.Repositories;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class UserEnrolledIntegrationEventHandler : IIntegrationEventHandler<UserEnrolledIntegrationEvent>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ILogger<UserEnrolledIntegrationEventHandler> _logger;

        public UserEnrolledIntegrationEventHandler(
            IWishlistRepository wishlistRepository,
            ILogger<UserEnrolledIntegrationEventHandler> logger)
        {
            _wishlistRepository = wishlistRepository;
            _logger = logger;
        }

        public async Task Handle(UserEnrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle UserEnrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var wishlist = await _wishlistRepository.FindByUserIdAsync(@event.UserId);

            if (wishlist != null)
            {
                wishlist.EnrollCourse(@event.Course.Id);
                wishlist.RemoveCourse(@event.Course.Id);

                await _wishlistRepository.UpdateWishlistAsync(wishlist);
            }
        }
    }
}
