using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class CommentCollectionSeeder : MongoCollectionSeeder<Comment>
    {
        public override async Task SeedAsync(IMongoCollection<Comment> collection)
        {
            var indexBuilder = Builders<Comment>.IndexKeys;

            var indexes = new List<CreateIndexModel<Comment>> {
                new (indexBuilder.Ascending(c => c.LessonId).Ascending(c => c.ReplyToId)),
                new (indexBuilder.Ascending(c => c.ReplyToId)),
            };

            await collection.Indexes.CreateManyAsync(indexes);
        }
    }
}
