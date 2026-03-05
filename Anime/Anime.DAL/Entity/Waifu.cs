using Anime.DAL.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Anime.DAL.Entity;

[Table("waifus")]
public class Waifu : BaseEntity
{
    [Column("surname")]
    [Required(ErrorMessage = "Surname IS required")]
    [StringLength(Constraints.MAX_NAME_LENGTH, MinimumLength = Constraints.MIN_NAME_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.NAME_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public string Surname { get; set; } = string.Empty;

    [Column("name")]
    [Required(ErrorMessage = "Name IS required")]
    [StringLength(Constraints.MAX_NAME_LENGTH, MinimumLength = Constraints.MIN_NAME_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.NAME_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public string Name { get; set; } = string.Empty;

    [Column("age")]
    [Range(Constraints.MIN_AGE, Constraints.MAX_AGE, ErrorMessage = ErrorMessages.AgeRange)]
    public int Age { get; set; }

    [Column("anime_id")]
    public Guid AnimeId { get; set; }

    [ForeignKey(nameof(AnimeId))]
    public virtual Anime? Anime { get; set; }
}
