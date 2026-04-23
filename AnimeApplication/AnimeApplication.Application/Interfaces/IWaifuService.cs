using AnimeApplication.Application.Common;
using AnimeApplication.Application.DTOs.Waifu;

namespace AnimeApplication.Application.Interfaces;

public interface IWaifuService {
    Task<Page<GetFullWaifuDTO>> GetAllAsync(int pageSize, int pageNum, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> AddWithAnimeIdAsync(CreateWaifuWithAnimeIdDTO dto, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> AddWithAnimeNameAsync(CreateWaifuWithAnimeNameDTO dto, CancellationToken ct = default);
    Task<GetFullWaifuDTO?> UpdateAsync(Guid id, UpdateWaifuDTO dto, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
    Task ForceDeleteAsync(Guid id, CancellationToken ct = default);
}