using AnimeApplication.Application.Common;
using AnimeApplication.Application.DTOs.Anime;

namespace AnimeApplication.Application.Interfaces;

public interface IAnimeService
{
    Task<Page<GetAnimeDTO>> GetAllAsync(int pageSize, int pageNum, CancellationToken ct = default);
    Task<GetAnimeDTO?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<GetAnimeDTO?> AddAsync(CreateAnimeDTO dto, CancellationToken ct = default);
    Task<GetAnimeDTO?> UpdateAsync(Guid id, UpdateAnimeDTO dto, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
    Task ForceDeleteAsync(Guid id, CancellationToken ct = default);
}
