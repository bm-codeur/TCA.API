using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChauffeursController : ControllerBase
{
    private readonly IChauffeurService _service;

    public ChauffeursController(IChauffeurService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChauffeurDto>>> GetAll()
    {
        var chauffeurs = await _service.GetAllAsync();
        return Ok(chauffeurs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChauffeurDto>> GetById(int id)
    {
        var chauffeur = await _service.GetByIdAsync(id);
        if (chauffeur is null) return NotFound();
        return Ok(chauffeur);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
    public async Task<ActionResult<ChauffeurDto>> Create([FromBody] CreateChauffeurDto dto)
    {
        var chauffeur = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = chauffeur.Id }, chauffeur);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
    public async Task<ActionResult<ChauffeurDto>> Update(int id, [FromBody] UpdateChauffeurDto dto)
    {
        var chauffeur = await _service.UpdateAsync(id, dto);
        if (chauffeur is null) return NotFound();
        return Ok(chauffeur);
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
