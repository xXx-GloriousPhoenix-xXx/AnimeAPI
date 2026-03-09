using Anime.BLL.DTO.Waifu;

namespace Anime.BLL.DTO.Anime;

public class GetAnimeDTO : BaseAnimeDTO
{
    public Guid Id { get; set; }
    public ICollection<GetWaifuDTO> Waifus { get; set; } = [];
}
