using Microsoft.AspNetCore.Mvc;
using Anime.BLL.Service.Interface;
using Anime.BLL.DTO.Anime;

namespace Anime.API.Controllers;

[Route("api/anime")]
[ApiController]
public class AnimeController(IAnimeService service) : ControllerBase
{
    private readonly IAnimeService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAnimeAsync(CancellationToken ct = default)
    {
        var animeList = await _service.GetAllAsync(ct);
        return Ok(animeList);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct = default)
    {
        var anime = await _service.GetByIdAsync(id, ct);
        return Ok(anime);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm] CreateAnimeDTO dto, CancellationToken ct = default)
    {
        var anime = await _service.AddAsync(dto, ct);
        return Ok(anime);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateAnimeDTO dto, CancellationToken ct = default)
    {
        var anime = await _service.UpdateAsync(id, dto, ct);
        return Ok(anime);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _service.DeleteAsync(id, ct);
        return Ok();
    }
}
