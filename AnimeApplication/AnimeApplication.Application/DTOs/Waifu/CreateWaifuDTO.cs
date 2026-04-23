using System.ComponentModel.DataAnnotations;
using AnimeApplication.Domain.Constraints;
using AnimeApplication.Application.ErrorMessages;

namespace AnimeApplication.Application.DTOs.Waifu;

public abstract class CreateWaifuDTO : BaseWaifuDTO {
    [StringLength(WaifuConstraints.MAX_NAME_LENGTH, MinimumLength = WaifuConstraints.MIN_NAME_LENGTH, ErrorMessage = CommonErrorMessages.StringLength)]
    [RegularExpression(WaifuConstraints.NAME_REGEXP, ErrorMessage = CommonErrorMessages.RegexPattern)]
    public override string? Surname { get; set; } = string.Empty;

    [StringLength(WaifuConstraints.MAX_NAME_LENGTH, MinimumLength = WaifuConstraints.MIN_NAME_LENGTH, ErrorMessage = CommonErrorMessages.StringLength)]
    [RegularExpression(WaifuConstraints.NAME_REGEXP, ErrorMessage = CommonErrorMessages.RegexPattern)]
    public override string? Name { get; set; } = string.Empty;

    [Range(WaifuConstraints.MIN_AGE, WaifuConstraints.MAX_AGE, ErrorMessage = WaifuErrorMessages.AgeRange)]
    public override int? Age { get; set; }
}
