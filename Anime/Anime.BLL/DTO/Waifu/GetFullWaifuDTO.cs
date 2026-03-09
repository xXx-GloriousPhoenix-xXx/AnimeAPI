namespace Anime.BLL.DTO.Waifu;

public class GetFullWaifuDTO : BaseWaifuDTO
{
    public Guid Id { get; set; }
    public string AnimeName { get; set; } = string.Empty;
}