using MongoDB.Driver;
using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public CourseRepository(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task<Course?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, id);

            return await _courseCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertCourseAsync(Course course)
        {
            await _courseCollection.InsertOneAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, course.Id);

            await _courseCollection.ReplaceOneAsync(filter, course);
        }

        public async Task UpdateStatusAsync(string courseId, CourseStatus status)
        {
            await _courseCollection.UpdateOneAsync(
                Builders<Course>.Filter.Eq(c => c.Id, courseId),
                Builders<Course>.Update.Set(c => c.Status, status)
            );
        }
    }
}
