using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupesController : ControllerBase
{
    private readonly IGroupeService _service;

    public GroupesController(IGroupeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupeDto>>> GetAll()
    {
        var groupes = await _service.GetAllAsync();
        return Ok(groupes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GroupeDto>> GetById(int id)
    {
        var groupe = await _service.GetByIdAsync(id);
        if (groupe is null) return NotFound();
        return Ok(groupe);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<GroupeDto>> Create([FromBody] CreateGroupeDto dto)
    {
        var groupe = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = groupe.Id }, groupe);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<GroupeDto>> Update(int id, [FromBody] UpdateGroupeDto dto)
    {
        var groupe = await _service.UpdateAsync(id, dto);
        if (groupe is null) return NotFound();
        return Ok(groupe);
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
