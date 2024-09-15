using System.Linq.Expressions;

namespace CourseService.Domain.Constracts
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T?> FindOneAsync(string id);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);

        Task<List<T>> GetAllAsync();

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteOneAsync(string id);
        Task DeleteOneAsync(Expression<Func<T, bool>> filter);

        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null);
    }
}
