using Anime.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anime.DAL.Context.Configuration;

public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uniqueidentifier")
                .IsRequired()
                .ValueGeneratedOnAdd();
    }
}
