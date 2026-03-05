using Anime.DAL.Context.Configuration;
using Anime.DAL.Entity;
using Microsoft.EntityFrameworkCore;
namespace Anime.DAL.Context;

public class AnimeDbContext(DbContextOptions<AnimeDbContext> options) : DbContext(options)
{
    public DbSet<Waifu> Waifus { get; set; }
    public DbSet<Entity.Anime> Animes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WaifuConfiguration());
        modelBuilder.ApplyConfiguration(new AnimeConfiguration());
    }
}
