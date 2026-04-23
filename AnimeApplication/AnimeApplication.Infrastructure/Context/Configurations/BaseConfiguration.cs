using AnimeApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApplication.Infrastructure.Context.Configurations;

public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity {
    public virtual void Configure(EntityTypeBuilder<T> builder) {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uniqueidentifier")
                .IsRequired()
                .ValueGeneratedOnAdd();
    }
}
