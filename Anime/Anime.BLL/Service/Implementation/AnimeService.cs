using Anime.BLL.DTO.Anime;
using Anime.BLL.Service.Interface;
using Anime.DAL.Repository.Interface;

using AutoMapper;

namespace Anime.BLL.Service.Implementation;

public class AnimeService(IUnitOfWork unitOfWork, IMapper mapper) : IAnimeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetAnimeDTO?> AddAsync(CreateAnimeDTO dto, CancellationToken ct = default)
    {
        var anime = _mapper.Map<DAL.Entity.Anime>(dto);
        _unitOfWork.Animes.Add(anime);
        var created = await _unitOfWork.Animes.GetByIdAsync(anime.Id, ct);
        var result = _mapper.Map<GetAnimeDTO>(created);
        return result;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var anime = await _unitOfWork.Animes.GetByIdAsync(id, ct);
        if (anime is not null)
        {
            _unitOfWork.Animes.Delete(anime);
        }
    }

    public async Task<IEnumerable<GetAnimeDTO>> GetAllAsync(CancellationToken ct = default)
    {
        var animeList = await _unitOfWork.Animes.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<GetAnimeDTO>>(animeList);
        return result;
    }

    public async Task<GetAnimeDTO?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var anime = await _unitOfWork.Animes.GetByIdAsync(id, ct);
        var result = _mapper.Map<GetAnimeDTO>(anime);
        return result;
    }

    public async Task<GetAnimeDTO?> UpdateAsync(Guid id, UpdateAnimeDTO dto, CancellationToken ct = default)
    {
        var anime = _mapper.Map<DAL.Entity.Anime>(dto);
        anime.Id = id;
        _unitOfWork.Animes.Update(anime);
        var updated = await _unitOfWork.Animes.GetByIdAsync(anime.Id, ct);
        var result = _mapper.Map<GetAnimeDTO>(updated);
        return result;
    }
}

