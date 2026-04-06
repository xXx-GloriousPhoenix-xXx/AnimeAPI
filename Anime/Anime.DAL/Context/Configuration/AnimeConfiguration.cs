using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Anime.DAL.Entity;

namespace Anime.DAL.Context.Configuration;
public class AnimeConfiguration : BaseConfiguration<Entity.AnimeEntity>
{
    public override void Configure(EntityTypeBuilder<Entity.AnimeEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("animes");

        builder.Property(a => a.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(a => a.ReleaseDate)
            .HasColumnName("release_date")
            .IsRequired();

        builder.Property(a => a.EpisodeCount)
            .HasColumnName("episode_count");

        builder
            .HasMany(a => a.Waifus)
            .WithOne(w => w.Anime)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
