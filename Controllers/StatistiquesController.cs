using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
public class StatistiquesController : ControllerBase
{
    private readonly IStatistiquesService _service;

    public StatistiquesController(IStatistiquesService service)
    {
        _service = service;
    }

    [HttpGet("journalieres")]
    public async Task<ActionResult<StatistiquesJournalieresDto>> GetJournalieres([FromQuery] DateTime? date)
    {
        var stats = await _service.GetJournalieresAsync(date);
        return Ok(stats);
    }

    [HttpGet("mensuelles")]
    public async Task<ActionResult<StatistiquesMensuellesDto>> GetMensuelles(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var stats = await _service.GetMensuellesAsync(mois, annee);
        return Ok(stats);
    }
}
