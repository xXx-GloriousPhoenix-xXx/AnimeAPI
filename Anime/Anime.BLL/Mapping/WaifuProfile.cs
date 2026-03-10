using AutoMapper;
using Anime.DAL.Entity;
using Anime.BLL.DTO.Waifu;

namespace Anime.BLL.Mapping;
public class WaifuProfile : Profile
{
    public WaifuProfile()
    {
        CreateMap<CreateWaifuWithAnimeIdDTO, Waifu>();
        CreateMap<CreateWaifuWithAnimeNameDTO, Waifu>();
        CreateMap<UpdateWaifuDTO, Waifu>();
        CreateMap<Waifu, GetFullWaifuDTO>();
    }
}
