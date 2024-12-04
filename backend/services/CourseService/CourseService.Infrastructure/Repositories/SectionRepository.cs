using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(IMongoCollection<Section> collection, IDomainEventDispatcher dispatcher) : 
            base(collection, dispatcher) { }

        public async Task<long> CountByCourseIdAsync(string courseId)
        {
            var filter = Builders<Section>.Filter.Eq(s => s.CourseId, courseId);

            return await _collection.CountDocumentsAsync(filter);
        }
    }
}
