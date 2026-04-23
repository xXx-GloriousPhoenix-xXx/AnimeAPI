using System.Linq.Expressions;
using AnimeApplication.Domain.Entities;
using AnimeApplication.Domain.Interfaces;
using AnimeApplication.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApplication.Infrastructure.Repositories;

public class BaseRepository<T>(AnimeDbContext context) : IBaseRepository<T> where T : BaseEntity {
    protected readonly AnimeDbContext _dbContext = context
            ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public void Add(T entity) {
        _dbSet.Add(entity);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default) {
        if (predicate is null) {
            return await _dbSet.CountAsync(ct);
        }
        return await _dbSet.CountAsync(predicate, ct);
    }

    public void Delete(T entity) {
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) {
        return await _dbSet.AnyAsync(predicate, ct);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) {
        return await _dbSet.Where(predicate).ToListAsync(ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default) {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default, params Expression<Func<T, object?>>[] includes) {
        if (includes == null || includes.Length == 0) {
            return await GetByIdAsync(id, ct);
        }

        IQueryable<T> query = _dbSet;
        foreach (var include in includes) {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public void Update(T entity) {
        _dbSet.Update(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default, params Expression<Func<T, object?>>[] includes) {
        if (includes == null || includes.Length == 0) {
            return await GetAllAsync(ct);
        }

        IQueryable<T> query = _dbSet;
        foreach (var include in includes) {
            query = query.Include(include);
        }

        return await query.ToListAsync(ct);
    }
}
