using AnimeApplication.Application.DTOs;

namespace AnimeApplication.Application.Common;

public class Page<T> where T : BaseDTO {
    public IEnumerable<T> Items { get; set; } = [];
    public int ItemCount { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public int PageNum { get; set; } = 1;
    public int PageCount { get; set; } = 0;
}
