namespace Anime.BLL.DTO.Extra;
public class Page<T> where T : BaseDTO
{
    public IEnumerable<T> Items { get; set; } = [];
    public int ItemCount { get; set; }
    public int PageSize { get; set; }
    public int PageNum { get; set; }
    public int PageCount { get; set; }
}
