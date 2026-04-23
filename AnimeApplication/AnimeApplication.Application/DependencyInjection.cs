using AnimeApplication.Application.Interfaces;
using AnimeApplication.Application.Mappings;
using AnimeApplication.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeApplication.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
         services.AddAutoMapper(cfg => cfg.AddProfiles([
            new WaifuProfile(),
            new AnimeProfile()
        ]));

         services.AddScoped<IWaifuService, WaifuService>();
         services.AddScoped<IAnimeService, AnimeService>();

        return services;
    }
}
