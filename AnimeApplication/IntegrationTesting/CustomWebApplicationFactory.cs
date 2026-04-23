using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using AnimeApplication.Infrastructure.Context;
using AnimeApplication.Domain.Entities;

namespace IntegrationTesting;

public class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";
    public static readonly Guid ExistingAnimeId1 = Guid.NewGuid();
    public static readonly Guid ExistingAnimeId2 = Guid.NewGuid();
    public static readonly Guid ExistingWaifuId = Guid.NewGuid();
    public static readonly Guid DeletedAnimeId = Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AnimeDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(AnimeDbContext) ||
                    d.ServiceType.FullName?.StartsWith("Microsoft.EntityFrameworkCore") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            services.AddDbContext<AnimeDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));
        });

        builder.ConfigureServices(services => {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AnimeDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData(db);
        });
    }

    private static void SeedData(AnimeDbContext db) {
        if (db.Set<Anime>().Any()) return;

        var anime1 = new Anime {
            Id = ExistingAnimeId1,
            Title = "Naruto",
            EpisodeCount = 220,
            ReleaseDate = new DateOnly(2002, 10, 3),
            IsDeleted = false
        };
        var anime2 = new Anime {
            Id = ExistingAnimeId2,
            Title = "Naruto",
            EpisodeCount = 220,
            ReleaseDate = new DateOnly(2002, 10, 3),
            IsDeleted = false
        };
        var deletedAnime = new Anime {
            Id = DeletedAnimeId,
            Title = "Deleted Anime",
            EpisodeCount = 1,
            ReleaseDate = new DateOnly(2000, 1, 1),
            IsDeleted = true
        };
        var waifu = new Waifu {
            Id = ExistingWaifuId,
            Name = "Hinata",
            Surname = "Hyuga",
            Age = 17,
            AnimeId = ExistingAnimeId1,
            IsDeleted = false
        };

        db.Set<Anime>().AddRange(anime1, anime2, deletedAnime);
        db.Set<Waifu>().Add(waifu);
        db.SaveChanges();
    }
}
