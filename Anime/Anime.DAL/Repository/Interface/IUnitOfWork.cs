using Anime.DAL.Entity;

namespace Anime.DAL.Repository.Interface;
public interface IUnitOfWork
{
    IBaseRepository<Waifu> Waifus { get; }
    IBaseRepository<Entity.AnimeEntity> Animes { get; }
    Task<int> CompleteAsync(CancellationToken ct =  default);
}
