using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(IMongoCollection<Enrollment> collection) : base(collection) { }
    }
}
