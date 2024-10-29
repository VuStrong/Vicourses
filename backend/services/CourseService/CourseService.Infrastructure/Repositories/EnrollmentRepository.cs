using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(IMongoCollection<Enrollment> collection) : base(collection) { }

        public async Task<PagedResult<Enrollment>> FindByUserIdAsync(string userId, int skip, int limit)
        {
            var filter = Builders<Enrollment>.Filter.Eq(e => e.UserId, userId);

            var enrollments = await _collection.Find(filter).SortByDescending(e => e.EnrolledAt)
                .Skip(skip).Limit(limit).ToListAsync();
            
            var total = await _collection.CountDocumentsAsync(filter);

            return new PagedResult<Enrollment>(enrollments, skip, limit, total);
        }
    }
}
