using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class LessionCollectionSeeder : MongoCollectionSeeder<Lession>
    {
        public override async Task SeedAsync(IMongoCollection<Lession> collection)
        {
            var indexBuilder = Builders<Lession>.IndexKeys;

            var indexes = new List<CreateIndexModel<Lession>> {
                new (indexBuilder.Ascending(c => c.CourseId)),
                new (indexBuilder.Ascending(c => c.SectionId))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
