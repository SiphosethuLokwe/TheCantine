using System.Linq.Expressions;

namespace Cantina.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int skip, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, int skip, int take, CancellationToken cancellationToken); 
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(object id, CancellationToken cancellationToken);


    }
}
