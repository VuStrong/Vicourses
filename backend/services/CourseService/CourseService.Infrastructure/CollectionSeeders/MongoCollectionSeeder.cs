using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public abstract class MongoCollectionSeeder<T>
    {
        public abstract Task SeedAsync(IMongoCollection<T> collection);
    }
}
