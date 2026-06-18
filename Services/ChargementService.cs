using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCA.API.Data;
using TCA.API.DTOs;
using TCA.API.Models;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface IChargementService
{
    Task<IEnumerable<ChargementDto>> GetAllAsync();
    Task<ChargementDto?> GetByIdAsync(int id);
    Task<ChargementDto> EnregistrerDepartAsync(CreateDepartDto dto);
    Task<ChargementDto?> EnregistrerRetourAsync(int id, EnregistrerRetourDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ChargementService : IChargementService
{
    private static readonly TimeOnly HeureLimiteDepart = new(17, 30);
    private const decimal LitresParKm = 2m;

    private readonly IChargementRepository _chargementRepository;
    private readonly ICamionRepository _camionRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChargementService(
        IChargementRepository chargementRepository,
        ICamionRepository camionRepository,
        IZoneRepository zoneRepository,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _chargementRepository = chargementRepository;
        _camionRepository = camionRepository;
        _zoneRepository = zoneRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChargementDto>> GetAllAsync()
    {
        var chargements = await _chargementRepository.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<ChargementDto>>(chargements);
    }

    public async Task<ChargementDto?> GetByIdAsync(int id)
    {
        var chargement = await _chargementRepository.GetByIdWithDetailsAsync(id);
        return chargement is null ? null : _mapper.Map<ChargementDto>(chargement);
    }

    public async Task<ChargementDto> EnregistrerDepartAsync(CreateDepartDto dto)
    {
        var heureDepart = dto.HeureDepart ?? DateTime.UtcNow;

        if (TimeOnly.FromDateTime(heureDepart) > HeureLimiteDepart)
            throw new InvalidOperationException("Pas de nouveau départ après 17h30.");

            var camion = await _camionRepository.GetByIdWithChargementsAsync(dto.CamionId)
        ?? throw new InvalidOperationException("Camion introuvable.");

        if (camion.Groupe is null)
            throw new InvalidOperationException("Ce camion n'est assigné à aucun groupe.");

        var zoneId = camion.Groupe.ZoneId;

        var zone = await _zoneRepository.GetByIdAsync(zoneId)
            ?? throw new InvalidOperationException("Zone introuvable.");

        // Check if there is a chauffeur currently assigned to the truck
        var chauffeur = await _context.Chauffeurs
            .Include(c => c.Camion)
                .ThenInclude(c => c!.Groupe)
            .FirstOrDefaultAsync(c => c.CamionId == dto.CamionId)
            ?? throw new InvalidOperationException("Aucun chauffeur n'est assigné à ce camion.");

        var dernierChargement = await _chargementRepository.GetDernierChargementCamionAsync(dto.CamionId);
        if (dernierChargement is not null && !dernierChargement.EstRetourne)
            throw new InvalidOperationException("Un camion doit avoir un retour enregistré avant de repartir.");

        var toursDuJour = await _chargementRepository.CountByZoneAndDateAsync(zoneId, heureDepart);
        if (toursDuJour >= zone.ToursMaxParJour)
            throw new InvalidOperationException(
                $"Le nombre maximum de tours par jour ({zone.ToursMaxParJour}) pour la zone {zone.Nom} est atteint.");

        // --- Monthly limits validation ---
        var currentMonthStart = new DateTime(heureDepart.Year, heureDepart.Month, 1);
        var nextMonthStart = currentMonthStart.AddMonths(1);

        // 1. Chauffeur monthly limit
        var chauffeurChargementsCount = await _context.Chargements.CountAsync(c =>
            c.ChauffeurId == chauffeur.Id &&
            c.DateChargement >= currentMonthStart &&
            c.DateChargement < nextMonthStart);
        if (chauffeurChargementsCount >= zone.ChargementMaxMoisChauffeur)
            throw new InvalidOperationException(
                $"Le chauffeur {chauffeur.Nom} {chauffeur.Prenom} a atteint son nombre maximal de chargements pour ce mois ({zone.ChargementMaxMoisChauffeur}).");

        // 2. Groupe monthly limit
        var groupeId = chauffeur.Camion!.GroupeId;
        var groupCamionIds = await _context.Camions.Where(c => c.GroupeId == groupeId).Select(c => c.Id).ToListAsync();
        var groupChargementsCount = await _context.Chargements.CountAsync(c =>
            groupCamionIds.Contains(c.CamionId) &&
            c.DateChargement >= currentMonthStart &&
            c.DateChargement < nextMonthStart);
        if (groupChargementsCount >= zone.ChargementMaxMoisGroupe)
            throw new InvalidOperationException(
                $"Le groupe {chauffeur.Camion.Groupe!.Numero} a atteint son nombre maximal de chargements pour ce mois ({zone.ChargementMaxMoisGroupe}).");

        // 3. Zone monthly limit
        var zoneChargementsCount = await _context.Chargements.CountAsync(c =>
            c.ZoneId == zone.Id &&
            c.DateChargement >= currentMonthStart &&
            c.DateChargement < nextMonthStart);
        if (zoneChargementsCount >= zone.ChargementMaxMoisZone)
            throw new InvalidOperationException(
                $"La zone {zone.Nom} a atteint son nombre maximal de chargements pour ce mois ({zone.ChargementMaxMoisZone}).");

        var distanceAllerRetour = zone.Distance * 2;

        var chargement = new Chargement
        {
            CamionId = dto.CamionId,
            ChauffeurId = chauffeur.Id, // Link current chauffeur
            ZoneId = zoneId,
            HeureDepart = heureDepart,
            DateChargement = heureDepart.Date,
            Kilometre = distanceAllerRetour,
            Carburant = distanceAllerRetour * LitresParKm,
            EstRetourne = false
        };

        camion.EstDisponible = false;
        _camionRepository.Update(camion);

        await _chargementRepository.AddAsync(chargement);
        await _chargementRepository.SaveChangesAsync();

        return (await GetByIdAsync(chargement.Id))!;
    }

    public async Task<ChargementDto?> EnregistrerRetourAsync(int id, EnregistrerRetourDto dto)
    {
        var chargement = await _chargementRepository.GetByIdWithDetailsAsync(id);
        if (chargement is null) return null;

        if (chargement.EstRetourne)
            throw new InvalidOperationException("Ce chargement a déjà un retour enregistré.");

        var heureRetour = dto.HeureRetour ?? DateTime.UtcNow;
        if (heureRetour <= chargement.HeureDepart)
            throw new InvalidOperationException("L'heure de retour doit être postérieure à l'heure de départ.");

        chargement.HeureRetour = heureRetour;
        chargement.EstRetourne = true;

        var camion = await _camionRepository.GetByIdAsync(chargement.CamionId);
        if (camion is not null)
        {
            camion.EstDisponible = true;
            _camionRepository.Update(camion);
        }

        _chargementRepository.Update(chargement);
        await _chargementRepository.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var chargement = await _chargementRepository.GetByIdAsync(id);
        if (chargement is null) return false;

        _chargementRepository.Delete(chargement);
        await _chargementRepository.SaveChangesAsync();
        return true;
    }
}
