using AnimeApplication.Application.DTOs.Waifu;
using AnimeApplication.Domain.Entities;
using AutoMapper;

namespace AnimeApplication.Application.Mappings;

public class WaifuProfile : Profile {
    public WaifuProfile() {
        CreateMap<CreateWaifuWithAnimeIdDTO, Waifu>();
        CreateMap<CreateWaifuWithAnimeNameDTO, Waifu>();
        CreateMap<UpdateWaifuDTO, Waifu>();
        CreateMap<Waifu, GetWaifuDTO>();
        CreateMap<Waifu, GetFullWaifuDTO>()
            .ForMember(dest => dest.AnimeName, opt => opt.MapFrom(a => a.Anime!.Title));
    }
}
