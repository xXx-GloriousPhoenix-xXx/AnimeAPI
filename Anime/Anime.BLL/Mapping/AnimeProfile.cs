using Anime.BLL.DTO.Anime;
using AutoMapper;
using Anime.DAL.Entity;
namespace Anime.BLL.Mapping;

public class AnimeProfile : Profile
{
    public AnimeProfile()
    {
        CreateMap<CreateAnimeDTO, AnimeEntity>();
        CreateMap<UpdateAnimeDTO, AnimeEntity>();
        CreateMap<AnimeEntity, GetAnimeDTO>();
    }
}
