using MongoDB.Driver;
using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.CollectionSeeders
{
    public class WishlistCollectionSeeder : MongoCollectionSeeder<Wishlist>
    {
        public override async Task SeedAsync(IMongoCollection<Wishlist> collection)
        {
            var indexBuilder = Builders<Wishlist>.IndexKeys;

            var indexes = new List<CreateIndexModel<Wishlist>> {
                new (indexBuilder.Ascending(x => x.UserId), new () { Unique = true }),
                new (indexBuilder.Ascending("Courses._id"))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
