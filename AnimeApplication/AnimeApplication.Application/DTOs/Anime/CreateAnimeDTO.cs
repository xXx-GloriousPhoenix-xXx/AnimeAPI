using System.ComponentModel.DataAnnotations;
using AnimeApplication.Domain.Constraints;
using AnimeApplication.Application.ErrorMessages;

namespace AnimeApplication.Application.DTOs.Anime;

public class CreateAnimeDTO : BaseAnimeDTO
{
    [Required(ErrorMessage = "Title IS required")]
    [StringLength(AnimeConstraints.MAX_TITLE_LENGTH, MinimumLength = AnimeConstraints.MIN_TITLE_LENGTH, ErrorMessage = CommonErrorMessages.StringLength)]
    [RegularExpression(AnimeConstraints.TITLE_REGEXP, ErrorMessage = CommonErrorMessages.RegexPattern)]
    public override string? Title { get; set; }

    [Required(ErrorMessage = "Release date IS required")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Range(typeof(DateOnly), AnimeConstraints.MIN_DATE, AnimeConstraints.MAX_DATE, ErrorMessage = AnimeErrorMessages.DateInterval)]
    public override DateOnly? ReleaseDate { get; set; }

    [Required(ErrorMessage = "Episode quantity IS required")]
    [Range(AnimeConstraints.MIN_EPISODES, int.MaxValue, ErrorMessage = AnimeErrorMessages.EpisodeCount)]
    public override int? EpisodeCount { get; set; }
}