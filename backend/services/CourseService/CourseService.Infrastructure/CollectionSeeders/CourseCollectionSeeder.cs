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
                new (indexBuilder.Text(c => c.Title)),
                new (indexBuilder.Ascending(c => c.Category.Id)),
                new (indexBuilder.Ascending(c => c.SubCategory.Id)),
                new (indexBuilder.Ascending(c => c.User.Id)),
                new (indexBuilder.Ascending(c => c.Status)),
                new (indexBuilder.Ascending(c => c.Tags)),
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
