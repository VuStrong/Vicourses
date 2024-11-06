using EventBus;
using MongoDB.Driver;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Models;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class UserEnrolledIntegrationEventHandler : IIntegrationEventHandler<UserEnrolledIntegrationEvent>
    {
        private readonly IMongoCollection<Wishlist> _wishlistCollection;
        private readonly ILogger<UserEnrolledIntegrationEventHandler> _logger;

        public UserEnrolledIntegrationEventHandler(
            IMongoCollection<Wishlist> wishlistCollection,
            ILogger<UserEnrolledIntegrationEventHandler> logger)
        {
            _wishlistCollection = wishlistCollection;
            _logger = logger;
        }

        public async Task Handle(UserEnrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle UserEnrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var wishlist = await _wishlistCollection
                .Find(Builders<Wishlist>.Filter.Eq(x => x.UserId, @event.UserId))
                .FirstOrDefaultAsync();

            if (wishlist != null)
            {
                wishlist.EnrollCourse(@event.Course.Id);
                wishlist.RemoveCourse(@event.Course.Id);

                await _wishlistCollection.ReplaceOneAsync(
                    Builders<Wishlist>.Filter.Eq(x => x.Id, wishlist.Id),
                    wishlist
                );
            }
        }
    }
}
