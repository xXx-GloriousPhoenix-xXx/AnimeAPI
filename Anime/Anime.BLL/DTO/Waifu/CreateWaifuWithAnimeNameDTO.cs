using System.ComponentModel.DataAnnotations;
using Anime.BLL.Validation;

namespace Anime.BLL.DTO.Waifu;

public class CreateWaifuWithAnimeNameDTO : CreateWaifuDTO
{
    [Required(ErrorMessage = "Surname IS required")]
    public override string? Surname { get; set; }

    [Required(ErrorMessage = "Name IS required")]
    public override string? Name { get; set; }

    [Required(ErrorMessage = "Age IS required")]
    public override int? Age { get; set; }

    [Required(ErrorMessage = "Title IS required")]
    [StringLength(Constraints.MAX_TITLE_LENGTH, MinimumLength = Constraints.MIN_TITLE_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.TITLE_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public string AnimeTitle { get; set; } = string.Empty;
}
