using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoCollection<User> collection, IDomainEventDispatcher dispatcher) : 
            base(collection, dispatcher) { } 
    }
}
