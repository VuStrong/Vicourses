using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class EnrollmentCollectionSeeder : MongoCollectionSeeder<Enrollment>
    {
        public override async Task SeedAsync(IMongoCollection<Enrollment> collection)
        {
            var indexBuilder = Builders<Enrollment>.IndexKeys;

            var index = new CreateIndexModel<Enrollment>(
                indexBuilder.Ascending(c => c.CourseId).Ascending(c => c.UserId),
                new () { Unique = true }
            );

            await collection.Indexes.CreateOneAsync(index);
        }
    }
}
