using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(IMongoCollection<Lesson> collection) : base(collection) { }

        public async Task<List<Lesson>> FindBySectionIdAsync(string sectionId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.SectionId, sectionId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Lesson>> FindByCourseIdAsync(string courseId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.CourseId, courseId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task DeleteBySectionIdAsync(string sectionId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.SectionId, sectionId);
            await _collection.DeleteManyAsync(filter);
        }

        public async Task DeleteByCourseIdAsync(string courseId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.CourseId, courseId);
            await _collection.DeleteManyAsync(filter);
        }
    }
}
