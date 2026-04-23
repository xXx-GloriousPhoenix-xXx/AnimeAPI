using AnimeApplication.Application.DTOs.Anime;
using AnimeApplication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApplication.API.Controllers;

[Route("api/animes")]
[ApiController]
public class AnimeController(IAnimeService service) : ControllerBase {
    private readonly IAnimeService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNum = 1,
        CancellationToken ct = default) {
        var animeList = await _service.GetAllAsync(pageSize, pageNum, ct);
        return Ok(animeList);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct = default) {
        var anime = await _service.GetByIdAsync(id, ct);
        return Ok(anime);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm] CreateAnimeDTO dto, CancellationToken ct = default) {
        var anime = await _service.AddAsync(dto, ct);
        return Ok(anime);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateAnimeDTO dto, CancellationToken ct = default) {
        var anime = await _service.UpdateAsync(id, dto, ct);
        return Ok(anime);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromQuery] bool isSoft = true,
        CancellationToken ct = default) {
        try {
            if (isSoft) {
                await _service.SoftDeleteAsync(id, ct);
            }
            else {
                await _service.ForceDeleteAsync(id, ct);
            }
            return Ok();
        }
        catch (KeyNotFoundException) {
            return NotFound();
        }
    }
}
