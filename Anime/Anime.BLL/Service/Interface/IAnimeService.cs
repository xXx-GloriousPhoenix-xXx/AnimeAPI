using Anime.BLL.DTO.Anime;

namespace Anime.BLL.Service.Interface;
public interface IAnimeService
{
    Task<IEnumerable<GetAnimeDTO>> GetAllAsync(CancellationToken ct = default);
    Task<GetAnimeDTO?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetAnimeDTO?> AddAsync(CreateAnimeDTO dto, CancellationToken ct = default);
    Task<GetAnimeDTO?> UpdateAsync(Guid id, UpdateAnimeDTO dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
