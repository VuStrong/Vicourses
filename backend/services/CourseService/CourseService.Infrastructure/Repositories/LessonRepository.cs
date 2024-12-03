using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(IMongoCollection<Lesson> collection, IDomainEventDispatcher dispatcher) 
            : base(collection, dispatcher) { }

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

        public async Task<long> CountBySectionIdAsync(string sectionId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.SectionId, sectionId);

            return await _collection.CountDocumentsAsync(filter);
        }

        public async Task<long> CountByCourseIdAsync(string courseId)
        {
            var filter = Builders<Lesson>.Filter.Eq(l => l.CourseId, courseId);

            return await _collection.CountDocumentsAsync(filter);
        }
    }
}
