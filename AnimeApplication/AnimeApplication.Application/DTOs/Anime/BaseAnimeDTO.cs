namespace AnimeApplication.Application.DTOs.Anime;

public abstract class BaseAnimeDTO : BaseDTO
{
    public virtual string? Title { get; set; }
    public virtual DateOnly? ReleaseDate { get; set; }
    public virtual int? EpisodeCount { get; set; }
}
