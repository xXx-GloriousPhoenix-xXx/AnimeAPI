using Anime.DAL.Entity;
using System.Linq.Expressions;
namespace Anime.DAL.Repository.Interface;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
