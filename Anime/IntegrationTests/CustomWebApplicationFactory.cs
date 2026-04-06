using Anime.DAL.Context;
using Anime.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";
    public static readonly Guid ExistingAnimeId = Guid.NewGuid();
    public static readonly Guid ExistingWaifuId = Guid.NewGuid();
    public static readonly Guid DeletedAnimeId = Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
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

        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AnimeDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData(db);
        });
    }

    private static void SeedData(AnimeDbContext db)
    {
        if (db.Set<AnimeEntity>().Any()) return;

        var anime = new AnimeEntity
        {
            Id = ExistingAnimeId,
            Title = "Naruto",
            EpisodeCount = 220,
            ReleaseDate = new DateOnly(2002, 10, 3),
            IsDeleted = false
        };
        var deletedAnime = new AnimeEntity
        {
            Id = DeletedAnimeId,
            Title = "Deleted Anime",
            EpisodeCount = 1,
            ReleaseDate = new DateOnly(2000, 1, 1),
            IsDeleted = true
        };
        var waifu = new Waifu
        {
            Id = ExistingWaifuId,
            Name = "Hinata",
            Surname = "Hyuga",
            Age = 17,
            AnimeId = ExistingAnimeId,
            IsDeleted = false
        };

        db.Set<AnimeEntity>().AddRange(anime, deletedAnime);
        db.Set<Waifu>().Add(waifu);
        db.SaveChanges();
    }
}
