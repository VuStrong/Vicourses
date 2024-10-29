using CourseService.Domain;
using CourseService.Domain.Contracts;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : IBaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<T?> FindOneAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<T>> FindByIdsAsync(IEnumerable<string> ids, bool sortByIds = true)
        {
            var filter = Builders<T>.Filter.In("_id", ids);
            var items = await _collection.Find(filter).ToListAsync();

            if (sortByIds && items.Count > 1)
            {
                var sortedItems = new List<T>();

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

        public async Task<bool> ExistsAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);

            return await _collection.Find(filter).AnyAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null)
        {
            filter ??= (_) => true;

            return await _collection.Find(filter).AnyAsync();
        }

        public async Task DeleteOneAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);

            await _collection.DeleteOneAsync(filter);
        }

        public async Task DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            await _collection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);

            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
