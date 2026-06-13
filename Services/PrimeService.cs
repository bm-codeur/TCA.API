using Microsoft.EntityFrameworkCore;
using TCA.API.Data;
using TCA.API.DTOs;
using TCA.API.Models;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface IPrimeService
{
    Task<IEnumerable<PrimeChauffeurDto>> GetPrimesChauffeursAsync(int? mois, int? annee);
    Task<IEnumerable<PrimeSuperviseurGroupeDto>> GetPrimesSuperviseursGroupeAsync(int? mois, int? annee);
    Task<IEnumerable<PrimeSuperviseurZoneDto>> GetPrimesSuperviseursZoneAsync(int? mois, int? annee);
    Task<PrimeSuperviseurGeneralDto?> GetPrimeSuperviseurGeneralAsync(int? mois, int? annee);
}

public class PrimeService : IPrimeService
{
    private readonly ApplicationDbContext _context;
    private readonly ISuperviseurGroupeRepository _superviseurGroupeRepository;
    private readonly ISuperviseurZoneRepository _superviseurZoneRepository;
    private readonly ISuperviseurGeneralRepository _superviseurGeneralRepository;

    public PrimeService(
        ApplicationDbContext context,
        ISuperviseurGroupeRepository superviseurGroupeRepository,
        ISuperviseurZoneRepository superviseurZoneRepository,
        ISuperviseurGeneralRepository superviseurGeneralRepository)
    {
        _context = context;
        _superviseurGroupeRepository = superviseurGroupeRepository;
        _superviseurZoneRepository = superviseurZoneRepository;
        _superviseurGeneralRepository = superviseurGeneralRepository;
    }

    public async Task<IEnumerable<PrimeChauffeurDto>> GetPrimesChauffeursAsync(int? mois, int? annee)
    {
        var (start, end) = GetDateRange(mois, annee);
        var chauffeurs = await _context.Chauffeurs
            .Include(c => c.Camion)
            .ThenInclude(c => c!.Groupe)
            .ThenInclude(g => g!.Zone)
            .ToListAsync();

        var result = new List<PrimeChauffeurDto>();

        foreach (var chauffeur in chauffeurs)
        {
            var nbChargements = await _context.Chargements
                .CountAsync(c =>
                    c.EstRetourne &&
                    c.CamionId == chauffeur.CamionId &&
                    c.DateChargement >= start &&
                    c.DateChargement <= end);

            var zone = chauffeur.Camion?.Groupe?.Zone;
            var primeUnitaire = zone?.PrimeChauffeurParChargement ?? 0;
            var primeTotal = nbChargements * primeUnitaire;

            result.Add(new PrimeChauffeurDto
            {
                ChauffeurId = chauffeur.Id,
                Nom = chauffeur.Nom,
                Prenom = chauffeur.Prenom,
                NombreChargements = nbChargements,
                PrimeUnitaire = primeUnitaire,
                PrimeTotal = primeTotal,
                SalaireBase = chauffeur.SalaireBase,
                TotalRemuneration = chauffeur.SalaireBase + primeTotal
            });
        }

        return result;
    }

    public async Task<IEnumerable<PrimeSuperviseurGroupeDto>> GetPrimesSuperviseursGroupeAsync(int? mois, int? annee)
    {
        var (start, end) = GetDateRange(mois, annee);
        var superviseurs = await _superviseurGroupeRepository.GetAllWithGroupeAsync();
        var result = new List<PrimeSuperviseurGroupeDto>();

        foreach (var sup in superviseurs)
        {
            var camionIds = await _context.Camions
                .Where(c => c.GroupeId == sup.GroupeId)
                .Select(c => c.Id)
                .ToListAsync();

            var nbChargements = await _context.Chargements
                .CountAsync(c =>
                    c.EstRetourne &&
                    c.DateChargement >= start &&
                    c.DateChargement <= end &&
                    camionIds.Contains(c.CamionId));

            var zone = sup.Groupe?.Zone;
            var primeUnitaire = zone?.PrimeSuperviseurGroupeParChargement ?? 0;
            var primeTotal = nbChargements * primeUnitaire;

            result.Add(new PrimeSuperviseurGroupeDto
            {
                SuperviseurGroupeId = sup.Id,
                Nom = sup.Nom,
                Prenom = sup.Prenom,
                GroupeNumero = sup.Groupe?.Numero ?? string.Empty,
                NombreChargements = nbChargements,
                PrimeUnitaire = primeUnitaire,
                PrimeTotal = primeTotal,
                SalaireBase = sup.SalaireBase,
                TotalRemuneration = sup.SalaireBase + primeTotal
            });
        }

        return result;
    }

    public async Task<IEnumerable<PrimeSuperviseurZoneDto>> GetPrimesSuperviseursZoneAsync(int? mois, int? annee)
    {
        var (start, end) = GetDateRange(mois, annee);
        var superviseurs = await _superviseurZoneRepository.GetAllWithZoneAsync();
        var result = new List<PrimeSuperviseurZoneDto>();

        foreach (var sup in superviseurs)
        {
            var nbChargements = await _context.Chargements
                .CountAsync(c =>
                    c.EstRetourne &&
                    c.ZoneId == sup.ZoneId &&
                    c.DateChargement >= start &&
                    c.DateChargement <= end);

            var primeUnitaire = sup.Zone?.PrimeSuperviseurZoneParChargement ?? 0;
            var primeTotal = nbChargements * primeUnitaire;

            result.Add(new PrimeSuperviseurZoneDto
            {
                SuperviseurZoneId = sup.Id,
                Nom = sup.Nom,
                Prenom = sup.Prenom,
                ZoneNom = sup.Zone?.Nom ?? string.Empty,
                NombreChargements = nbChargements,
                PrimeUnitaire = primeUnitaire,
                PrimeTotal = primeTotal,
                SalaireBase = sup.SalaireBase,
                TotalRemuneration = sup.SalaireBase + primeTotal
            });
        }

        return result;
    }

    public async Task<PrimeSuperviseurGeneralDto?> GetPrimeSuperviseurGeneralAsync(int? mois, int? annee)
    {
        var superviseur = (await _superviseurGeneralRepository.GetAllAsync()).FirstOrDefault();
        if (superviseur is null) return null;

        var (start, end) = GetDateRange(mois, annee);
        var nbChargements = await _context.Chargements
            .CountAsync(c =>
                c.EstRetourne &&
                c.DateChargement >= start &&
                c.DateChargement <= end);

        var primeTotal = nbChargements * SuperviseurGeneral.PrimeParChargement;

        return new PrimeSuperviseurGeneralDto
        {
            SuperviseurGeneralId = superviseur.Id,
            Nom = superviseur.Nom,
            Prenom = superviseur.Prenom,
            NombreChargements = nbChargements,
            PrimeUnitaire = SuperviseurGeneral.PrimeParChargement,
            PrimeTotal = primeTotal,
            SalaireBase = superviseur.SalaireBase,
            TotalRemuneration = superviseur.SalaireBase + primeTotal
        };
    }

    private static (DateTime start, DateTime end) GetDateRange(int? mois, int? annee)
    {
        var now = DateTime.UtcNow;
        var m = mois ?? now.Month;
        var a = annee ?? now.Year;
        var start = new DateTime(a, m, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }
}
