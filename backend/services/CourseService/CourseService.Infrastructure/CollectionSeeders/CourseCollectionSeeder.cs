using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class CourseCollectionSeeder : MongoCollectionSeeder<Course>
    {
        public override async Task SeedAsync(IMongoCollection<Course> collection)
        {
            var indexBuilder = Builders<Course>.IndexKeys;

            var indexes = new List<CreateIndexModel<Course>> {
                new (indexBuilder.Ascending(c => c.Category.Id)),
                new (indexBuilder.Ascending(c => c.User.Id))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
