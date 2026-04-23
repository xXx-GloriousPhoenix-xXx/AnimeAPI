using AnimeApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApplication.Infrastructure.Context.Configurations;

public class AnimeConfiguration : BaseConfiguration<Anime> {
    public override void Configure(EntityTypeBuilder<Anime> builder) {
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