namespace AnimeApplication.Application.DTOs.Waifu; 

public class BaseWaifuDTO : BaseDTO
{
    public virtual string? Surname { get; set; }
    public virtual string? Name { get; set; }
    public virtual int? Age { get; set; }
}
