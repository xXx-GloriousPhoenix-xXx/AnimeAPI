using Anime.BLL.DTO.Waifu;
using Anime.BLL.Service.Interface;
using Anime.DAL.Entity;
using Anime.DAL.Repository.Interface;
using AutoMapper;

namespace Anime.BLL.Service.Implementation;

public class WaifuService(IUnitOfWork unitOfWork, IMapper mapper) : IWaifuService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<GetFullWaifuDTO?> AddWithAnimeIdAsync(CreateWaifuWithAnimeIdDTO dto, CancellationToken ct = default)
    {
        _ = await _unitOfWork.Animes.GetByIdAsync(dto.AnimeId, ct)
            ?? throw new ArgumentNullException(nameof(dto));

        var waifu = _mapper.Map<Waifu>(dto);
        _unitOfWork.Waifus.Add(waifu);
        var created = _unitOfWork.Waifus.GetByIdAsync(waifu.Id, ct);
        var result = _mapper.Map<GetFullWaifuDTO>(created);
        return result;
    }

    public async Task<GetFullWaifuDTO?> AddWithAnimeNameAsync(CreateWaifuWithAnimeNameDTO dto, CancellationToken ct = default)
    {
        var anime = await _unitOfWork.Animes.FindAsync(a => a.Title == dto.AnimeTitle, ct);
        if (!anime.Any())
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var waifu = _mapper.Map<Waifu>(dto);
        waifu.AnimeId = anime.First().Id;

        _unitOfWork.Waifus.Add(waifu);
        var created = _unitOfWork.Waifus.GetByIdAsync(waifu.Id, ct);
        var result = _mapper.Map<GetFullWaifuDTO>(created);
        return result;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var waifu = await _unitOfWork.Waifus.GetByIdAsync(id, ct);
        if (waifu is not null)
        {
            _unitOfWork.Waifus.Delete(waifu);
        }
    }

    public async Task<IEnumerable<GetFullWaifuDTO>> GetAllAsync(CancellationToken ct = default)
    {
        var waifuList = await _unitOfWork.Waifus.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<GetFullWaifuDTO>>(waifuList);
        return result;
    }

    public async Task<GetFullWaifuDTO?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var waifu = await _unitOfWork.Waifus.GetByIdAsync(id, ct);
        var result = _mapper.Map<GetFullWaifuDTO>(waifu);
        return result;
    }

    public async Task<GetFullWaifuDTO?> UpdateAsync(Guid id, UpdateWaifuDTO dto, CancellationToken ct = default)
    {
        var waifu = _mapper.Map<Waifu>(dto);
        waifu.Id = id;
        _unitOfWork.Waifus.Update(waifu);
        var updated = await _unitOfWork.Waifus.GetByIdAsync(id, ct);
        var result = _mapper.Map<GetFullWaifuDTO>(updated);
        return result;
    }
}
