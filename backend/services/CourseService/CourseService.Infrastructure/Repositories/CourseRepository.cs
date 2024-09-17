using CourseService.Domain.Constracts;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(IMongoCollection<Course> collection) : base(collection) {}

        public async Task<PagedResult<Course>> FindManyAsync(int skip, int limit, string? searchKeyword = null, string? categoryId = null,
            string? subCategoryId = null)
        {
            var builder = Builders<Course>.Filter;
            var filter = builder.Empty;
            bool haveQueryParams = false;

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter &= builder.Text(searchKeyword);
                haveQueryParams = true;
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                filter &= builder.Eq(c => c.Category.Id, categoryId);
                haveQueryParams = true;
            }

            if (!string.IsNullOrEmpty(subCategoryId))
            {
                filter &= builder.Eq(c => c.SubCategory.Id, subCategoryId);
                haveQueryParams = true;
            }

            var courses = await _collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();

            var countOptions = haveQueryParams ? null : new CountOptions { Hint = "_id_" };
            var total = await _collection.CountDocumentsAsync(filter, countOptions);

            return new PagedResult<Course>(courses, skip, limit, total);
        }

        public async Task IncreaseStudentCount(string courseId, int count)
        {
            var filter = Builders<Course>.Filter.Eq("_id", courseId);
            var update = Builders<Course>.Update.Inc(c => c.StudentCount, count);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
