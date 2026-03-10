using Microsoft.AspNetCore.Mvc;
using Anime.BLL.Service.Interface;
using Anime.BLL.DTO.Anime;
using Anime.BLL.DTO.Waifu;

namespace Anime.API.Controllers;

[Route("api/waifu")]
[ApiController]
public class WaifuController(IWaifuService service) : ControllerBase
{
    private readonly IWaifuService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct = default)
    {
        var waifuList = await _service.GetAllAsync(ct);
        return Ok(waifuList);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct = default)
    {
        var waifu = await _service.GetByIdAsync(id, ct);
        return Ok(waifu);
    }

    [HttpPost("id")]
    public async Task<IActionResult> AddWithAnimeIdAsync([FromForm] CreateWaifuWithAnimeIdDTO dto, CancellationToken ct = default)
    {
        var waifu = await _service.AddWithAnimeIdAsync(dto, ct);
        return Ok(waifu);
    }

    [HttpPost("name")]
    public async Task<IActionResult> AddWithAnimeNameAsync([FromForm] CreateWaifuWithAnimeNameDTO dto, CancellationToken ct = default)
    {
        var waifu = await _service.AddWithAnimeNameAsync(dto, ct);
        return Ok(waifu);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateWaifuDTO dto, CancellationToken ct = default)
    {
        var waifu = await _service.UpdateAsync(id, dto, ct);
        return Ok(waifu);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct = default)
    {
        await _service.DeleteAsync(id, ct);
        return Ok();
    }
}
