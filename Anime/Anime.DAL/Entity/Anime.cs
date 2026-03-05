using Anime.DAL.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Anime.DAL.Entity;

[Table("animes")]
public class Anime : BaseEntity
{
    [Column("title")]
    [Required(ErrorMessage = "Title IS required")]
    [StringLength(Constraints.MAX_TITLE_LENGTH, MinimumLength = Constraints.MIN_TITLE_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.TITLE_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public string Title { get; set; } = string.Empty;

    [Column("release_date", TypeName = "date")]
    [Required(ErrorMessage = "Release date IS required")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Range(typeof(DateOnly), Constraints.MIN_DATE, Constraints.MAX_DATE, ErrorMessage = ErrorMessages.DateInterval)]
    public DateOnly ReleaseDate { get; set; }

    [Column("episode_count")]
    [Range(Constraints.MIN_EPISODES, int.MaxValue, ErrorMessage = ErrorMessages.EpisodeCount)]
    public int EpisodeCount { get; set; }

    public virtual ICollection<Waifu> Waifus { get; set; } = [];
}
