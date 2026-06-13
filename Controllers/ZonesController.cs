using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ZonesController : ControllerBase
{
    private readonly IZoneService _service;

    public ZonesController(IZoneService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZoneDto>>> GetAll()
    {
        var zones = await _service.GetAllAsync();
        return Ok(zones);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ZoneDto>> GetById(int id)
    {
        var zone = await _service.GetByIdAsync(id);
        if (zone is null) return NotFound();
        return Ok(zone);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<ZoneDto>> Create([FromBody] CreateZoneDto dto)
    {
        var zone = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = zone.Id }, zone);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<ZoneDto>> Update(int id, [FromBody] UpdateZoneDto dto)
    {
        var zone = await _service.UpdateAsync(id, dto);
        if (zone is null) return NotFound();
        return Ok(zone);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
