using CourseService.Domain.Constracts;
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
            decimal? minimumRating = null)
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

            if (isPaid != null)
            {
                filter &= builder.Eq(c => c.IsPaid, isPaid);
                haveQueryParams = true;
            }

            if (level != null)
            {
                filter &= builder.Eq(c => c.Level, level);
                haveQueryParams = true;
            }

            if (minimumRating != null)
            {
                filter &= builder.Gte(c => c.Rating, minimumRating);
                haveQueryParams = true;
            }

            var fluent = _collection.Find(filter);

            if (sort != null)
            {
                fluent = Sort(fluent, sort ?? CourseSort.Newest);
            }

            var courses = await fluent.Skip(skip).Limit(limit).ToListAsync();

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
