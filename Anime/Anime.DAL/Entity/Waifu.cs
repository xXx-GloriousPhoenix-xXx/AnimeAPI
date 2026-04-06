using Anime.DAL.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Anime.DAL.Entity;

[Table("waifus")]
public class Waifu : BaseEntity
{
    [Column("surname")]
    public string Surname { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("age")]
    public int Age { get; set; }

    [Column("anime_id")]
    public Guid AnimeId { get; set; }

    [ForeignKey(nameof(AnimeId))]
    public virtual AnimeEntity? Anime { get; set; }
}
