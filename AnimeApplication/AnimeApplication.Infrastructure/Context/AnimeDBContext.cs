using Microsoft.EntityFrameworkCore;
using AnimeApplication.Domain.Entities;
using AnimeApplication.Infrastructure.Context.Configurations;

namespace AnimeApplication.Infrastructure.Context;

public class AnimeDbContext(DbContextOptions<AnimeDbContext> options) : DbContext(options) {
    public DbSet<Waifu> Waifus { get; set; }
    public DbSet<Anime> Animes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WaifuConfiguration());
        modelBuilder.ApplyConfiguration(new AnimeConfiguration());
    }
}