using System.Linq.Expressions;
using AnimeApplication.Domain.Entities;

namespace AnimeApplication.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default, params Expression<Func<T, object?>>[] includes);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default, params Expression<Func<T, object?>>[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    IQueryable<T> AsQueryable();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
