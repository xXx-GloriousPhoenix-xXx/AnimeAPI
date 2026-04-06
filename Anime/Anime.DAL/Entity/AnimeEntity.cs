using System.ComponentModel.DataAnnotations.Schema;
namespace Anime.DAL.Entity;

[Table("animes")]
public class AnimeEntity : BaseEntity
{
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("release_date", TypeName = "date")]
    public DateOnly ReleaseDate { get; set; }

    [Column("episode_count")]
    public int EpisodeCount { get; set; }

    public virtual ICollection<Waifu> Waifus { get; set; } = [];
}
