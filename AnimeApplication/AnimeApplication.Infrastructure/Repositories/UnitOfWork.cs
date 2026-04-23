using AnimeApplication.Domain.Entities;
using AnimeApplication.Domain.Interfaces;
using AnimeApplication.Infrastructure.Context;

namespace AnimeApplication.Infrastructure.Repositories;

public class UnitOfWork(AnimeDbContext context) : IUnitOfWork {
    private readonly AnimeDbContext _context = context;
    private IBaseRepository<Waifu>? _waifus;
    private IBaseRepository<Anime>? _animes;

    public IBaseRepository<Waifu> Waifus =>
        _waifus ??= new BaseRepository<Waifu>(_context);

    public IBaseRepository<Anime> Animes =>
        _animes ??= new BaseRepository<Anime>(_context);

    public async Task<int> CompleteAsync(CancellationToken ct = default) {
        return await _context.SaveChangesAsync(ct);
    }
}