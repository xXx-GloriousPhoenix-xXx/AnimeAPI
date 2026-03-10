using System.ComponentModel.DataAnnotations;
using Anime.BLL.Validation;

namespace Anime.BLL.DTO.Anime;

public class CreateAnimeDTO : BaseAnimeDTO
{
    [Required(ErrorMessage = "Title IS required")]
    [StringLength(Constraints.MAX_TITLE_LENGTH, MinimumLength = Constraints.MIN_TITLE_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.TITLE_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public override string? Title { get; set; }

    [Required(ErrorMessage = "Release date IS required")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Range(typeof(DateOnly), Constraints.MIN_DATE, Constraints.MAX_DATE, ErrorMessage = ErrorMessages.DateInterval)]
    public override DateOnly? ReleaseDate { get; set; }

    [Required(ErrorMessage = "Episode quantity IS required")]
    [Range(Constraints.MIN_EPISODES, int.MaxValue, ErrorMessage = ErrorMessages.EpisodeCount)]
    public override int? EpisodeCount { get; set; }
}
