using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class LessionRepository : Repository<Lession>, ILessionRepository
    {
        public LessionRepository(IMongoCollection<Lession> collection) : base(collection) { }

        public async Task<List<Lession>> FindBySectionIdAsync(string sectionId)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.SectionId, sectionId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Lession>> FindByCourseIdAsync(string courseId)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.CourseId, courseId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task DeleteBySectionIdAsync(string sectionId)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.SectionId, sectionId);
            await _collection.DeleteManyAsync(filter);
        }

        public async Task DeleteByCourseIdAsync(string courseId)
        {
            var filter = Builders<Lession>.Filter.Eq(l => l.CourseId, courseId);
            await _collection.DeleteManyAsync(filter);
        }
    }
}
