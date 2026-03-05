using Anime.DAL.Context;
using Anime.DAL.Entity;
using Anime.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Anime.DAL.Repository.Implementation;

public class BaseRepository<T>(AnimeDbContext context) : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AnimeDbContext _dbContext = context
            ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
