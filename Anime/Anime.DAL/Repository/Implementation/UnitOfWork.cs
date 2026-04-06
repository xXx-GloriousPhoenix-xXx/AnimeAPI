using Anime.DAL.Context;
using Anime.DAL.Entity;
using Anime.DAL.Repository.Interface;

namespace Anime.DAL.Repository.Implementation;

public class UnitOfWork(AnimeDbContext context) : IUnitOfWork
{
    private readonly AnimeDbContext _context = context;
    private IBaseRepository<Waifu>? _waifus;
    private IBaseRepository<Entity.AnimeEntity>? _animes;

    public IBaseRepository<Waifu> Waifus =>
        _waifus ??= new BaseRepository<Waifu>(_context);

    public IBaseRepository<Entity.AnimeEntity> Animes =>
        _animes ??= new BaseRepository<Entity.AnimeEntity>(_context);

    public async Task<int> CompleteAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
