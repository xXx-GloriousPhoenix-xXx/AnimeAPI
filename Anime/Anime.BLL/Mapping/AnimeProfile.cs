using Anime.BLL.DTO.Anime;
using AutoMapper;
namespace Anime.BLL.Mapping;

public class AnimeProfile : Profile
{
    public AnimeProfile()
    {
        CreateMap<CreateAnimeDTO, DAL.Entity.Anime>();
        CreateMap<UpdateAnimeDTO, DAL.Entity.Anime>();
        CreateMap<DAL.Entity.Anime, GetAnimeDTO>();
    }
}
