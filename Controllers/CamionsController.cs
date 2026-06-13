using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CamionsController : ControllerBase
{
    private readonly ICamionService _service;

    public CamionsController(ICamionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CamionDto>>> GetAll()
    {
        var camions = await _service.GetAllAsync();
        return Ok(camions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CamionDto>> GetById(int id)
    {
        var camion = await _service.GetByIdAsync(id);
        if (camion is null) return NotFound();
        return Ok(camion);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<CamionDto>> Create([FromBody] CreateCamionDto dto)
    {
        var camion = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = camion.Id }, camion);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<CamionDto>> Update(int id, [FromBody] UpdateCamionDto dto)
    {
        var camion = await _service.UpdateAsync(id, dto);
        if (camion is null) return NotFound();
        return Ok(camion);
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
