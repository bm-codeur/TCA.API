using Microsoft.EntityFrameworkCore;
using TCA.API.Data;
using TCA.API.DTOs;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface IStatistiquesService
{
    Task<StatistiquesJournalieresDto> GetJournalieresAsync(DateTime? date);
    Task<StatistiquesMensuellesDto> GetMensuellesAsync(int? mois, int? annee);
}

public class StatistiquesService : IStatistiquesService
{
    private readonly IChargementRepository _chargementRepository;
    private readonly ApplicationDbContext _context;

    public StatistiquesService(IChargementRepository chargementRepository, ApplicationDbContext context)
    {
        _chargementRepository = chargementRepository;
        _context = context;
    }

    public async Task<StatistiquesJournalieresDto> GetJournalieresAsync(DateTime? date)
    {
        var targetDate = (date ?? DateTime.UtcNow).Date;
        var start = targetDate;
        var end = targetDate.AddDays(1).AddTicks(-1);

        var chargements = (await _chargementRepository.GetByDateRangeAsync(start, end)).ToList();

        var parZone = chargements
            .GroupBy(c => new { c.ZoneId, c.Zone!.Nom })
            .Select(g => new StatistiqueZoneDto
            {
                ZoneId = g.Key.ZoneId,
                ZoneNom = g.Key.Nom,
                NombreChargements = g.Count(),
                TotalKilometres = g.Sum(c => c.Kilometre),
                TotalCarburant = g.Sum(c => c.Carburant),
                TotalRevenus = g.Sum(c => c.Zone!.TarifChargement)
            })
            .ToList();

        return new StatistiquesJournalieresDto
        {
            Date = targetDate,
            TotalChargements = chargements.Count,
            ChargementsRetournes = chargements.Count(c => c.EstRetourne),
            ChargementsEnCours = chargements.Count(c => !c.EstRetourne),
            TotalKilometres = chargements.Sum(c => c.Kilometre),
            TotalCarburant = chargements.Sum(c => c.Carburant),
            TotalRevenus = chargements.Sum(c => c.Zone!.TarifChargement),
            ParZone = parZone
        };
    }

    public async Task<StatistiquesMensuellesDto> GetMensuellesAsync(int? mois, int? annee)
    {
        var now = DateTime.UtcNow;
        var m = mois ?? now.Month;
        var a = annee ?? now.Year;
        var start = new DateTime(a, m, 1);
        var end = start.AddMonths(1).AddTicks(-1);

        var chargements = (await _chargementRepository.GetByDateRangeAsync(start, end)).ToList();

        var parZone = chargements
            .GroupBy(c => new { c.ZoneId, c.Zone!.Nom })
            .Select(g => new StatistiqueZoneDto
            {
                ZoneId = g.Key.ZoneId,
                ZoneNom = g.Key.Nom,
                NombreChargements = g.Count(),
                TotalKilometres = g.Sum(c => c.Kilometre),
                TotalCarburant = g.Sum(c => c.Carburant),
                TotalRevenus = g.Sum(c => c.Zone!.TarifChargement)
            })
            .ToList();

        var parGroupe = chargements
            .Where(c => c.Camion?.Groupe is not null)
            .GroupBy(c => new { c.Camion!.GroupeId, c.Camion.Groupe!.Numero })
            .Select(g => new StatistiqueGroupeDto
            {
                GroupeId = g.Key.GroupeId,
                GroupeNumero = g.Key.Numero,
                NombreChargements = g.Count(),
                TotalKilometres = g.Sum(c => c.Kilometre),
                TotalCarburant = g.Sum(c => c.Carburant)
            })
            .ToList();

        return new StatistiquesMensuellesDto
        {
            Mois = m,
            Annee = a,
            TotalChargements = chargements.Count,
            ChargementsRetournes = chargements.Count(c => c.EstRetourne),
            TotalKilometres = chargements.Sum(c => c.Kilometre),
            TotalCarburant = chargements.Sum(c => c.Carburant),
            TotalRevenus = chargements.Sum(c => c.Zone!.TarifChargement),
            ParZone = parZone,
            ParGroupe = parGroupe
        };
    }
}
