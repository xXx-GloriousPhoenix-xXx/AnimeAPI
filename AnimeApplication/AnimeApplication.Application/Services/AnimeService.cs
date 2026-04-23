using AnimeApplication.Application.Common;
using AnimeApplication.Application.DTOs.Anime;
using AnimeApplication.Application.Interfaces;
using AnimeApplication.Domain.Entities;
using AnimeApplication.Domain.Interfaces;
using AutoMapper;

namespace AnimeApplication.Application.Services;

public class AnimeService(IUnitOfWork unitOfWork, IMapper mapper) : IAnimeService {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetAnimeDTO?> AddAsync(CreateAnimeDTO dto, CancellationToken ct = default) {
        var anime = _mapper.Map<Anime>(dto);

        _unitOfWork.Animes.Add(anime);
        await _unitOfWork.CompleteAsync(ct);

        var created = await _unitOfWork.Animes.GetByIdAsync(anime.Id, ct);
        var result = _mapper.Map<GetAnimeDTO>(created);
        return result;
    }

    public async Task ForceDeleteAsync(Guid id, CancellationToken ct = default) {
        var anime = await _unitOfWork.Animes.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException(nameof(id));

        _unitOfWork.Animes.Delete(anime);
        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<Page<GetAnimeDTO>> GetAllAsync(int pageSize = 10, int pageNum = 1, CancellationToken ct = default) {
        var animeList = await _unitOfWork.Animes.GetAllAsync(ct, a => a.Waifus);
        var present = animeList
            .Where(a => !a.IsDeleted)
            .Select(a => {
                a.Waifus = [.. a.Waifus.Where(w => !w.IsDeleted)];
                return a;
            });

        var totalCount = present.Count();
        var totalPages = (totalCount + pageSize - 1) / pageSize;

        var portion = present
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);
        var result = _mapper.Map<IEnumerable<GetAnimeDTO>>(portion);

        var response = new Page<GetAnimeDTO>() {
            Items = result,
            ItemCount = totalCount,
            PageCount = totalPages,
            PageNum = pageNum
        };

        return response;
    }

    public async Task<GetAnimeDTO?> GetByIdAsync(Guid id, CancellationToken ct = default) {
        var anime = await _unitOfWork.Animes.GetByIdAsync(id, ct, a => a.Waifus);
        if (anime is null || anime.IsDeleted) {
            throw new ArgumentNullException(nameof(id));
        }

        var presentWaifus = anime.Waifus.Where(w => !w.IsDeleted).ToList();
        anime.Waifus = presentWaifus;

        var result = _mapper.Map<GetAnimeDTO>(anime);
        return result;
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default) {
        var current = await _unitOfWork.Animes.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException(nameof(id));

        current.IsDeleted = true;

        _unitOfWork.Animes.Update(current);
        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<GetAnimeDTO?> UpdateAsync(Guid id, UpdateAnimeDTO dto, CancellationToken ct = default) {
        var current = await _unitOfWork.Animes.GetByIdAsync(id, ct)
            ?? throw new ArgumentNullException(nameof(dto));

        if (dto.Title != null) {
            current.Title = dto.Title;
        }
        if (dto.ReleaseDate != null) {
            current.ReleaseDate = (DateOnly)dto.ReleaseDate;
        }
        if (dto.EpisodeCount != null) {
            current.EpisodeCount = (int)dto.EpisodeCount;
        }

        _unitOfWork.Animes.Update(current);
        await _unitOfWork.CompleteAsync(ct);

        var updated = await _unitOfWork.Animes.GetByIdAsync(id, ct, a => a.Waifus);
        var result = _mapper.Map<GetAnimeDTO>(updated);
        return result;
    }
}