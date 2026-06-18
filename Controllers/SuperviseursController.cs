using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuperviseursController : ControllerBase
{
    private readonly ISuperviseurGeneralService _generalService;
    private readonly ISuperviseurZoneService _zoneService;
    private readonly ISuperviseurGroupeService _groupeService;

    public SuperviseursController(
        ISuperviseurGeneralService generalService,
        ISuperviseurZoneService zoneService,
        ISuperviseurGroupeService groupeService)
    {
        _generalService = generalService;
        _zoneService = zoneService;
        _groupeService = groupeService;
    }

    // --- SUPERVISEURS GENERAUX ---

    [HttpGet("generaux")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<IEnumerable<SuperviseurGeneralDto>>> GetAllGeneraux()
    {
        var result = await _generalService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("generaux/{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<SuperviseurGeneralDto>> GetGeneralById(int id)
    {
        var result = await _generalService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("generaux")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuperviseurGeneralDto>> CreateGeneral([FromBody] CreateSuperviseurGeneralDto dto)
    {
        var result = await _generalService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetGeneralById), new { id = result.Id }, result);
    }

    [HttpPut("generaux/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SuperviseurGeneralDto>> UpdateGeneral(int id, [FromBody] UpdateSuperviseurGeneralDto dto)
    {
        var result = await _generalService.UpdateAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("generaux/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGeneral(int id)
    {
        var deleted = await _generalService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // --- SUPERVISEURS DE ZONE ---

    [HttpGet("zones")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<IEnumerable<SuperviseurZoneDto>>> GetAllZones()
    {
        var result = await _zoneService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("zones/{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<SuperviseurZoneDto>> GetZoneById(int id)
    {
        var result = await _zoneService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("zones")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<SuperviseurZoneDto>> CreateZone([FromBody] CreateSuperviseurZoneDto dto)
    {
        var result = await _zoneService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetZoneById), new { id = result.Id }, result);
    }

    [HttpPut("zones/{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<SuperviseurZoneDto>> UpdateZone(int id, [FromBody] UpdateSuperviseurZoneDto dto)
    {
        var result = await _zoneService.UpdateAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("zones/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteZone(int id)
    {
        var deleted = await _zoneService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // --- SUPERVISEURS DE GROUPE ---

    [HttpGet("groupes")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
    public async Task<ActionResult<IEnumerable<SuperviseurGroupeDto>>> GetAllGroupes()
    {
        var result = await _groupeService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("groupes/{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
    public async Task<ActionResult<SuperviseurGroupeDto>> GetGroupeById(int id)
    {
        var result = await _groupeService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("groupes")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<SuperviseurGroupeDto>> CreateGroupe([FromBody] CreateSuperviseurGroupeDto dto)
    {
        var result = await _groupeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetGroupeById), new { id = result.Id }, result);
    }

    [HttpPut("groupes/{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<SuperviseurGroupeDto>> UpdateGroupe(int id, [FromBody] UpdateSuperviseurGroupeDto dto)
    {
        var result = await _groupeService.UpdateAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("groupes/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGroupe(int id)
    {
        var deleted = await _groupeService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
