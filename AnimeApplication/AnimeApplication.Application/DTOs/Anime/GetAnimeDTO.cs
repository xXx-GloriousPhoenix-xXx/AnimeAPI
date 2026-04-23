using AnimeApplication.Application.DTOs.Waifu;

namespace AnimeApplication.Application.DTOs.Anime;

public class GetAnimeDTO : BaseAnimeDTO
{
    public Guid Id { get; set; }
    public ICollection<GetWaifuDTO> Waifus { get; set; } = [];
}
