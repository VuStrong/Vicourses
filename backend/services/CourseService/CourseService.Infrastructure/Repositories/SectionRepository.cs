using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(IMongoCollection<Section> collection) : base(collection) { }

        public async Task DeleteByCourseIdAsync(string courseId)
        {
            await _collection.DeleteManyAsync(Builders<Section>.Filter.Eq(s => s.CourseId, courseId));
        }
    }
}
