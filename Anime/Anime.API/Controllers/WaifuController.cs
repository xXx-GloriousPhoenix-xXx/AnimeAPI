using Microsoft.AspNetCore.Mvc;
using Anime.BLL.Service.Interface;
using Anime.BLL.DTO.Waifu;

namespace Anime.API.Controllers;

[Route("api/waifus")]
[ApiController]
public class WaifuController(IWaifuService service) : ControllerBase
{
    private readonly IWaifuService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNum = 1,
        CancellationToken ct = default)
    {
        var waifuList = await _service.GetAllAsync(pageSize, pageNum, ct);
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

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateWaifuDTO dto, CancellationToken ct = default)
    {
        var waifu = await _service.UpdateAsync(id, dto, ct);
        return Ok(waifu);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromQuery] bool isSoft = true,
        CancellationToken ct = default)
    {
        if (isSoft)
        {
            await _service.SoftDeleteAsync(id, ct);
        }
        else
        {
            await _service.ForceDeleteAsync(id, ct);
        }
        return Ok();
    }
}
