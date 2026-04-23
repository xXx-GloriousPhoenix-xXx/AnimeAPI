using AnimeApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApplication.Infrastructure.Context.Configurations;

public class WaifuConfiguration : BaseConfiguration<Waifu> {
    public override void Configure(EntityTypeBuilder<Waifu> builder) {
        base.Configure(builder);

        builder.ToTable("waifus");

        builder.Property(w => w.Surname)
            .HasColumnName("surname");

        builder.Property(w => w.Name)
            .HasColumnName("name");

        builder.Property(w => w.Age)
            .HasColumnName("age");

        builder
            .HasOne(w => w.Anime)
            .WithMany(a => a.Waifus)
            .OnDelete(DeleteBehavior.Cascade);
    }
}