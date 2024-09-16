using CourseService.Domain.Constracts;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(IMongoCollection<Course> collection) : base(collection) {}

        public async Task IncreaseStudentCount(string courseId, int count)
        {
            var filter = Builders<Course>.Filter.Eq("_id", courseId);
            var update = Builders<Course>.Update.Inc(c => c.StudentCount, count);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
