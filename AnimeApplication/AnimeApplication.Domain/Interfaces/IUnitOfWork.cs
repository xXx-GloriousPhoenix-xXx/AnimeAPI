using AnimeApplication.Domain.Entities;

namespace AnimeApplication.Domain.Interfaces;

public interface IUnitOfWork
{
    IBaseRepository<Waifu> Waifus { get; }
    IBaseRepository<Anime> Animes { get; }
    Task<int> CompleteAsync(CancellationToken ct = default);
}