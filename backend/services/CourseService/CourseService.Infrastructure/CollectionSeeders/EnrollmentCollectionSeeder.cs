using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class EnrollmentCollectionSeeder : MongoCollectionSeeder<Enrollment>
    {
        public override async Task SeedAsync(IMongoCollection<Enrollment> collection)
        {
            var indexBuilder = Builders<Enrollment>.IndexKeys;

            var indexes = new List<CreateIndexModel<Enrollment>>
            {
                new ( indexBuilder.Ascending(c => c.CourseId).Ascending(c => c.UserId),
                    new () { Unique = true } ),
                new ( indexBuilder.Ascending(c => c.UserId) )
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
