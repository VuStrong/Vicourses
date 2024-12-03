using CourseService.Domain;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CourseService.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, IBaseEntity
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IDomainEventDispatcher _dispatcher;

        public Repository(IMongoCollection<T> collection, IDomainEventDispatcher dispatcher)
        {
            _collection = collection;
            _dispatcher = dispatcher;
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);

            _ = _dispatcher.DispatchFrom(entity);
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

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);

            await _collection.ReplaceOneAsync(filter, entity);

            _ = _dispatcher.DispatchFrom(entity);
        }
    }
}
