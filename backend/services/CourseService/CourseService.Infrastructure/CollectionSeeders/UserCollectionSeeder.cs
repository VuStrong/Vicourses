using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.CollectionSeeders
{
    public class UserCollectionSeeder : MongoCollectionSeeder<User>
    {
        public override async Task SeedAsync(IMongoCollection<User> collection)
        {
            
        }
    }
}
