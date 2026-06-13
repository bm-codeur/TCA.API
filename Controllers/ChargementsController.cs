using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChargementsController : ControllerBase
{
    private readonly IChargementService _service;

    public ChargementsController(IChargementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChargementDto>>> GetAll()
    {
        var chargements = await _service.GetAllAsync();
        return Ok(chargements);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChargementDto>> GetById(int id)
    {
        var chargement = await _service.GetByIdAsync(id);
        if (chargement is null) return NotFound();
        return Ok(chargement);
    }

    [HttpPost("depart")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe,Chauffeur")]
    public async Task<ActionResult<ChargementDto>> EnregistrerDepart([FromBody] CreateDepartDto dto)
    {
        try
        {
            var chargement = await _service.EnregistrerDepartAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = chargement.Id }, chargement);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}/retour")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe,Chauffeur")]
    public async Task<ActionResult<ChargementDto>> EnregistrerRetour(int id, [FromBody] EnregistrerRetourDto dto)
    {
        try
        {
            var chargement = await _service.EnregistrerRetourAsync(id, dto);
            if (chargement is null) return NotFound();
            return Ok(chargement);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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
