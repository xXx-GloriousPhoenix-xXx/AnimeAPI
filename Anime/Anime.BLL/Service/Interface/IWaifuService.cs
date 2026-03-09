using Anime.BLL.DTO.Anime;
using Anime.BLL.DTO.Waifu;

namespace Anime.BLL.Service.Interface;
public interface IWaifuService
{
    Task<IEnumerable<GetFullWaifuDTO>> GetAllAsync(CancellationToken ct = default);
    Task<GetFullWaifuDTO?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> AddWithAnimeIdAsync(CreateWaifuWithAnimeIdDTO dto, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> AddWithAnimeNameAsync(CreateWaifuWithAnimeNameDTO dto, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> UpdateAsync(Guid id, UpdateWaifuDTO dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
