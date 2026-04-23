using System.ComponentModel.DataAnnotations;
using AnimeApplication.Application.ErrorMessages;
using AnimeApplication.Domain.Constraints;

namespace AnimeApplication.Application.DTOs.Waifu;

public class CreateWaifuWithAnimeNameDTO : CreateWaifuDTO {
    [Required(ErrorMessage = "Surname IS required")]
    public override string? Surname { get; set; }

    [Required(ErrorMessage = "Name IS required")]
    public override string? Name { get; set; }

    [Required(ErrorMessage = "Age IS required")]
    public override int? Age { get; set; }

    [Required(ErrorMessage = "Title IS required")]
    [StringLength(AnimeConstraints.MAX_TITLE_LENGTH, MinimumLength = AnimeConstraints.MIN_TITLE_LENGTH, ErrorMessage = CommonErrorMessages.StringLength)]
    [RegularExpression(AnimeConstraints.TITLE_REGEXP, ErrorMessage = CommonErrorMessages.RegexPattern)]
    public string AnimeTitle { get; set; } = string.Empty;
}
