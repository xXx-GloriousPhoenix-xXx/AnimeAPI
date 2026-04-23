using System.ComponentModel.DataAnnotations;

namespace AnimeApplication.Application.DTOs.Waifu;

public class CreateWaifuWithAnimeIdDTO : CreateWaifuDTO {
    [Required(ErrorMessage = "Surname IS required")]
    public override string? Surname { get; set; }

    [Required(ErrorMessage = "Name IS required")]
    public override string? Name { get; set; }

    [Required(ErrorMessage = "Age IS required")]
    public override int? Age { get; set; }

    [Required(ErrorMessage = "Anime id IS required")]
    public Guid AnimeId { get; set; }
}
