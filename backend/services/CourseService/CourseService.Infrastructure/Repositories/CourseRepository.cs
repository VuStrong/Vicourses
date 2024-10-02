using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(IMongoCollection<Course> collection) : base(collection) {}

        public async Task<PagedResult<Course>> FindManyAsync(int skip, int limit, CourseSort? sort = null, string? searchKeyword = null, 
            string? categoryId = null, string? subCategoryId = null, bool? isPaid = null, CourseLevel? level = null, 
            decimal? minimumRating = null, CourseStatus status = CourseStatus.Published)
        {
            var builder = Builders<Course>.Filter;
            var filter = builder.Eq(c => c.Status, status);

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter &= builder.Text(searchKeyword);
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                filter &= builder.Eq(c => c.Category.Id, categoryId);
            }

            if (!string.IsNullOrEmpty(subCategoryId))
            {
                filter &= builder.Eq(c => c.SubCategory.Id, subCategoryId);
            }

            if (isPaid != null)
            {
                filter &= builder.Eq(c => c.IsPaid, isPaid);
            }

            if (level != null)
            {
                filter &= builder.Eq(c => c.Level, level);
            }

            if (minimumRating != null)
            {
                filter &= builder.Gte(c => c.Rating, minimumRating);
            }

            var fluent = _collection.Find(filter);

            if (sort != null)
            {
                fluent = Sort(fluent, sort ?? CourseSort.Newest);
            }

            var courses = await fluent.Skip(skip).Limit(limit).ToListAsync();

            var total = await _collection.CountDocumentsAsync(filter);

            return new PagedResult<Course>(courses, skip, limit, total);
        }

        public async Task<PagedResult<Course>> FindManyByUserIdAsync(string userId, int skip, int limit, string? searchKeyword = null,
            CourseStatus? status = null)
        {
            var builder = Builders<Course>.Filter;
            var filter = builder.Eq(c => c.User.Id, userId);

            if (status != null)
            {
                filter &= builder.Eq(c => c.Status, status);
            }

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                filter &= builder.Text(searchKeyword);
            }

            var courses = await _collection.Find(filter)
                .SortByDescending(c => c.CreatedAt).Skip(skip).Limit(limit).ToListAsync();
            var total = await _collection.CountDocumentsAsync(filter);

            return new PagedResult<Course>(courses, skip, limit, total);
        }

        public async Task UpdateStudentCountAsync(Course course)
        {
            var filter = Builders<Course>.Filter.Eq("_id", course.Id);
            var update = Builders<Course>.Update.Set(c => c.StudentCount, course.StudentCount);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserInCoursesAsync(UserInCourse user)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.User.Id, user.Id);
            var update = Builders<Course>.Update.Set(c => c.User, user);

            await _collection.UpdateManyAsync(filter, update);
        }

        private IFindFluent<Course, Course> Sort(IFindFluent<Course, Course> fluent, CourseSort sort)
        {
            switch (sort)
            {
                case CourseSort.Newest:
                    return fluent.SortByDescending(c => c.UpdatedAt);
                case CourseSort.HighestRated:
                    return fluent.SortByDescending(c => c.Rating);
                case CourseSort.PriceDesc:
                    return fluent.SortByDescending(c => c.Price);
                case CourseSort.PriceAsc:
                    return fluent.SortBy(c => c.Price);
                default:
                    throw new ArgumentException("Invalid sort value", nameof(sort));
            }
        }
    }
}
