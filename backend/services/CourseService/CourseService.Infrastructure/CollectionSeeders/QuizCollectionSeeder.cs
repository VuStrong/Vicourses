using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class QuizCollectionSeeder : MongoCollectionSeeder<Quiz>
    {
        public override async Task SeedAsync(IMongoCollection<Quiz> collection)
        {
            var indexBuilder = Builders<Quiz>.IndexKeys;

            var indexes = new List<CreateIndexModel<Quiz>> {
                new (indexBuilder.Ascending(c => c.LessionId))
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
