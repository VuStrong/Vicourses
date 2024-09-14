using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class CategoryCollectionSeeder : MongoCollectionSeeder<Category>
    {
        public override async Task SeedAsync(IMongoCollection<Category> collection)
        {
            var indexBuilder = Builders<Category>.IndexKeys;

            var indexes = new List<CreateIndexModel<Category>> {
                new (indexBuilder.Ascending(c => c.Slug),
                    new () { Unique = true }
                ),
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
