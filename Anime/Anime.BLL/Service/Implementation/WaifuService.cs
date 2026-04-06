using Anime.BLL.DTO.Extra;
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
        await _unitOfWork.CompleteAsync(ct);

        var created = await _unitOfWork.Waifus.GetByIdAsync(waifu.Id, ct);
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
        await _unitOfWork.CompleteAsync(ct);

        var created = await _unitOfWork.Waifus.GetByIdAsync(waifu.Id, ct);
        var result = _mapper.Map<GetFullWaifuDTO>(created);
        return result;
    }

    public async Task ForceDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var waifu = await _unitOfWork.Waifus.GetByIdAsync(id, ct)
            ?? throw new ArgumentNullException(nameof(id));

        _unitOfWork.Waifus.Delete(waifu);
        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<Page<GetFullWaifuDTO>> GetAllAsync(int pageSize = 10, int pageNum = 1, CancellationToken ct = default)
    {
        var waifuList = await _unitOfWork.Waifus.GetAllAsync(ct, w => w.Anime);
        var present = waifuList.Where(w => !w.IsDeleted);

        var totalCount = present.Count();
        var totalPages = (totalCount + pageSize - 1) / pageSize;

        var portion = present
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);
        var result = _mapper.Map<IEnumerable<GetFullWaifuDTO>>(portion);

        var response = new Page<GetFullWaifuDTO>()
        {
            Items = result,
            ItemCount = totalCount,
            PageCount = totalPages,
            PageNum = pageNum
        };

        return response;
    }

    public async Task<GetFullWaifuDTO?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var waifu = await _unitOfWork.Waifus.GetByIdAsync(id, ct, w => w.Anime);
        if (waifu is null ||  waifu.IsDeleted)
        {
            throw new ArgumentNullException(nameof(id));
        }
        var result = _mapper.Map<GetFullWaifuDTO>(waifu);
        return result;
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var current = await _unitOfWork.Waifus.GetByIdAsync(id, ct)
            ?? throw new ArgumentNullException(nameof(id));

        current.IsDeleted = true;

        _unitOfWork.Waifus.Update(current);
        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<GetFullWaifuDTO?> UpdateAsync(Guid id, UpdateWaifuDTO dto, CancellationToken ct = default)
    {
        var current = await _unitOfWork.Waifus.GetByIdAsync(id, ct)
            ?? throw new ArgumentNullException(nameof(dto));
        
        if (dto.Surname != null)
        {
            current.Surname = dto.Surname;
        }
        if (dto.Name != null)
        {
            current.Name = dto.Name;
        }
        if (dto.Age != null)
        {
            current.Age = (int)dto.Age;
        }

        _unitOfWork.Waifus.Update(current);
        await _unitOfWork.CompleteAsync(ct);

        var updated = await _unitOfWork.Waifus.GetByIdAsync(id, ct, w => w.Anime);
        var result = _mapper.Map<GetFullWaifuDTO>(updated);
        return result;
    }
}
