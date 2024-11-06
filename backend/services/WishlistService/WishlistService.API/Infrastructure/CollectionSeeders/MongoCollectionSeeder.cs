using MongoDB.Driver;

namespace WishlistService.API.Infrastructure.CollectionSeeders
{
    public abstract class MongoCollectionSeeder<T>
    {
        public abstract Task SeedAsync(IMongoCollection<T> collection);
    }
}
