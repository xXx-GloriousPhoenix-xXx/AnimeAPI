using Anime.BLL.DTO.Extra;

namespace Anime.BLL.DTO.Waifu;

public abstract class BaseWaifuDTO : BaseDTO
{
    public virtual string? Surname { get; set; }
    public virtual string? Name { get; set; }
    public virtual int? Age { get; set; }
}
