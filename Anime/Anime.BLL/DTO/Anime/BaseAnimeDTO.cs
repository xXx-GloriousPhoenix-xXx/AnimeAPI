namespace Anime.BLL.DTO.Anime;

public abstract class BaseAnimeDTO
{
    public string Title { get; set; } = string.Empty;
    public DateOnly ReleaseDate { get; set; }
    public int EpisodeCount { get; set; }
}
 