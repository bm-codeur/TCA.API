using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCA.API.Data;
using TCA.API.DTOs;
using TCA.API.Services.Interfaces;

namespace TCA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrimesController : ControllerBase
{
    private readonly IPrimeService _service;
    private readonly ApplicationDbContext _context;

    public PrimesController(IPrimeService service, ApplicationDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet("me")]
    [Authorize(Roles = "Chauffeur")]
    public async Task<ActionResult<PrimeChauffeurDto>> GetMyPrime(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim is null) return Unauthorized(new { message = "Non authentifié." });

        var userId = int.Parse(userIdClaim.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user is null || !user.ChauffeurId.HasValue)
            return BadRequest(new { message = "Ce compte utilisateur n'est lié à aucun chauffeur." });

        var prime = await _service.GetPrimeChauffeurByIdAsync(user.ChauffeurId.Value, mois, annee);
        if (prime is null) return NotFound(new { message = "Chauffeur non trouvé." });

        return Ok(prime);
    }

    [HttpGet("chauffeurs")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone,SuperviseurGroupe")]
    public async Task<ActionResult<IEnumerable<PrimeChauffeurDto>>> GetPrimesChauffeurs(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesChauffeursAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseurs-groupe")]
    [Authorize(Roles = "Admin,SuperviseurGeneral,SuperviseurZone")]
    public async Task<ActionResult<IEnumerable<PrimeSuperviseurGroupeDto>>> GetPrimesSuperviseursGroupe(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesSuperviseursGroupeAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseurs-zone")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<IEnumerable<PrimeSuperviseurZoneDto>>> GetPrimesSuperviseursZone(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var primes = await _service.GetPrimesSuperviseursZoneAsync(mois, annee);
        return Ok(primes);
    }

    [HttpGet("superviseur-general")]
    [Authorize(Roles = "Admin,SuperviseurGeneral")]
    public async Task<ActionResult<PrimeSuperviseurGeneralDto>> GetPrimeSuperviseurGeneral(
        [FromQuery] int? mois,
        [FromQuery] int? annee)
    {
        var prime = await _service.GetPrimeSuperviseurGeneralAsync(mois, annee);
        if (prime is null) return NotFound();
        return Ok(prime);
    }
}
