using System.Linq.Expressions;

namespace CourseService.Domain.Constracts
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);

        Task<List<T>> GetAllAsync();

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteOneAsync(Expression<Func<T, bool>> filter);

        Task<bool> ExistAsync(Expression<Func<T, bool>>? filter = null);
    }
}
