using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class SectionCollectionSeeder : MongoCollectionSeeder<Section>
    {
        public override async Task SeedAsync(IMongoCollection<Section> collection)
        {
            var indexBuilder = Builders<Section>.IndexKeys;

            var indexes = new List<CreateIndexModel<Section>> {
                new (indexBuilder.Ascending(c => c.CourseId))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
