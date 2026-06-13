using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrimesController : ControllerBase
{
    private readonly IPrimeService _service;

    public PrimesController(IPrimeService service)
    {
        _service = service;
    }

    [HttpGet("chauffeurs")]
    public async Task<ActionResult<IEnumerable<PrimeChauffeurDto>>> GetPrimesChauffeurs(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesChauffeursAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseurs-groupe")]
    public async Task<ActionResult<IEnumerable<PrimeSuperviseurGroupeDto>>> GetPrimesSuperviseursGroupe(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesSuperviseursGroupeAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseurs-zone")]
    public async Task<ActionResult<IEnumerable<PrimeSuperviseurZoneDto>>> GetPrimesSuperviseursZone(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesSuperviseursZoneAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseur-general")]
    public async Task<ActionResult<PrimeSuperviseurGeneralDto>> GetPrimeSuperviseurGeneral(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var prime = await _service.GetPrimeSuperviseurGeneralAsync(mois, annee);
        if (prime is null) return NotFound();
        return Ok(prime);
    }
}
