using AnimeApplication.Application.DTOs.Anime;
using AnimeApplication.Domain.Entities;
using AutoMapper;

namespace AnimeApplication.Application.Mappings;

public class AnimeProfile : Profile {
    public AnimeProfile() {
        CreateMap<CreateAnimeDTO, Anime>();
        CreateMap<UpdateAnimeDTO, Anime>();
        CreateMap<Anime, GetAnimeDTO>();
    }
}
