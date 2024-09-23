using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoCollection<User> collection) : base(collection) { } 
    }
}
