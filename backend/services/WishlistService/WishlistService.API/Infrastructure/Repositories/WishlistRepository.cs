using MongoDB.Driver;
using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly IMongoCollection<Wishlist> _wishlistCollection;

        public WishlistRepository(IMongoCollection<Wishlist> wishlistCollection)
        {
            _wishlistCollection = wishlistCollection;
        }

        public async Task<Wishlist?> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Wishlist>.Filter.Eq(x => x.UserId, userId);

            return await _wishlistCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> CheckCourseInWishlistAsync(string userId, string courseId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<Wishlist>.Filter;
            var filter = builder.Eq(x => x.UserId, userId) & builder.Eq("Courses._id", courseId);

            return await _wishlistCollection.Find(filter).AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetCourseIdsInWishlistAsync(string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Wishlist>.Filter.Eq(x => x.UserId, userId);
            var projection = Builders<Wishlist>.Projection.Include("Courses._id");

            var wishlist = await _wishlistCollection
                .Find(filter)
                .Project<Wishlist>(projection)
                .FirstOrDefaultAsync(cancellationToken);
            if (wishlist == null)
            {
                return [];
            }

            return wishlist.Courses.Select(c => c.Id);
        }

        public async Task InsertWishlistAsync(Wishlist wishlist)
        {
            await _wishlistCollection.InsertOneAsync(wishlist);
        }

        public async Task UpdateWishlistAsync(Wishlist wishlist)
        {
            var filter = Builders<Wishlist>.Filter.Eq(x => x.Id, wishlist.Id);

            await _wishlistCollection.ReplaceOneAsync(filter, wishlist);
        }

        public async Task UpdateCourseInWishlistsAsync(Course course)
        {
            var filter = Builders<Wishlist>.Filter.Eq("Courses._id", course.Id);
            var update = Builders<Wishlist>.Update.Set("Courses.$", course);

            await _wishlistCollection.UpdateManyAsync(filter, update);
        }

        public async Task RemoveCourseInWishlistsAsync(string courseId)
        {
            var filter = Builders<Wishlist>.Filter.Eq("Courses._id", courseId);
            var update = Builders<Wishlist>.Update
                .Inc(x => x.Count, -1)
                .PullFilter("Courses", Builders<Course>.Filter.Eq("_id", courseId));

            await _wishlistCollection.UpdateManyAsync(filter, update);
        }
    }
}
