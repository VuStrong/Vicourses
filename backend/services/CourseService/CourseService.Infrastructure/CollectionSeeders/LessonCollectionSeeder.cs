using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class LessonCollectionSeeder : MongoCollectionSeeder<Lesson>
    {
        public override async Task SeedAsync(IMongoCollection<Lesson> collection)
        {
            var indexBuilder = Builders<Lesson>.IndexKeys;

            var indexes = new List<CreateIndexModel<Lesson>> {
                new (indexBuilder.Ascending(c => c.CourseId)),
                new (indexBuilder.Ascending(c => c.SectionId))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
