namespace Anime.BLL.DTO.Waifu;

public abstract class BaseWaifuDTO
{
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
