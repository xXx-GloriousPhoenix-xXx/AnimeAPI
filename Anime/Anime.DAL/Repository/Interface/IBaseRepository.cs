using Anime.DAL.Entity;
namespace Anime.DAL.Repository.Interface;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
