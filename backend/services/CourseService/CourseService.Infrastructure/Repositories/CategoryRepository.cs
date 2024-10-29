using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMongoCollection<Category> collection) : base(collection) { }

        public async Task<List<Category>> GetAllAsync(string? keyword = null, string? parentId = null)
        {
            var builder = Builders<Category>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(parentId))
            {
                filter &= builder.Eq(c => c.ParentId, parentId == "null" ? null : parentId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                filter &= builder.Regex(c => c.Name, new BsonRegularExpression($".*{keyword}.*", "i"));
            }

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
