using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(IMongoCollection<Course> collection, IDomainEventDispatcher dispatcher) : 
            base(collection, dispatcher) {}

        public async Task<PagedResult<Course>> FindManyAsync(int skip, int limit, CourseSort? sort = null, string? searchKeyword = null, 
            string? categoryId = null, string? subCategoryId = null, bool? isPaid = null, CourseLevel? level = null, 
            decimal? minimumRating = null, CourseStatus status = CourseStatus.Published, string? tag = null)
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

            if (tag != null)
            {
                filter &= builder.AnyEq(c => c.Tags, tag);
            }

            var fluent = _collection.Find(filter);

            if (sort != null)
            {
                fluent = Sort(fluent, sort.Value);
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

        public async Task<List<Course>> FindByIdsAsync(IEnumerable<string> ids, CourseStatus? status = null, bool sortByIds = true)
        {
            var filter = Builders<Course>.Filter.In("_id", ids);

            if (status != null)
            {
                filter &= Builders<Course>.Filter.Eq(c => c.Status, status.Value);
            }

            var items = await _collection.Find(filter).ToListAsync();

            if (sortByIds && items.Count > 1)
            {
                var sortedItems = new List<Course>();

                foreach (var id in ids)
                {
                    var item = items.FirstOrDefault(i => i.Id == id);

                    if (item == null) continue;

                    sortedItems.Add(item);
                }

                return sortedItems;
            }

            return items;
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
