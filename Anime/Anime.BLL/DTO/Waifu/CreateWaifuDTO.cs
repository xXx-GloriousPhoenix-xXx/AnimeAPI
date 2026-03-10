using System.ComponentModel.DataAnnotations;
using Anime.BLL.Validation;

namespace Anime.BLL.DTO.Waifu;

public abstract class CreateWaifuDTO : BaseWaifuDTO
{
    [StringLength(Constraints.MAX_NAME_LENGTH, MinimumLength = Constraints.MIN_NAME_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.NAME_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public override string? Surname { get; set; } = string.Empty;

    [StringLength(Constraints.MAX_NAME_LENGTH, MinimumLength = Constraints.MIN_NAME_LENGTH, ErrorMessage = ErrorMessages.StringLength)]
    [RegularExpression(Constraints.NAME_REGEXP, ErrorMessage = ErrorMessages.RegexPattern)]
    public override string? Name { get; set; } = string.Empty;

    [Range(Constraints.MIN_AGE, Constraints.MAX_AGE, ErrorMessage = ErrorMessages.AgeRange)]
    public override int? Age { get; set; }
}
