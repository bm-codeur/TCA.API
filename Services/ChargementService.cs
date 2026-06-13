using AutoMapper;
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
    private readonly IMapper _mapper;

    public ChargementService(
        IChargementRepository chargementRepository,
        ICamionRepository camionRepository,
        IZoneRepository zoneRepository,
        IMapper mapper)
    {
        _chargementRepository = chargementRepository;
        _camionRepository = camionRepository;
        _zoneRepository = zoneRepository;
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

        var zone = await _zoneRepository.GetByIdAsync(dto.ZoneId)
            ?? throw new InvalidOperationException("Zone introuvable.");

        var dernierChargement = await _chargementRepository.GetDernierChargementCamionAsync(dto.CamionId);
        if (dernierChargement is not null && !dernierChargement.EstRetourne)
            throw new InvalidOperationException("Un camion doit avoir un retour enregistré avant de repartir.");

        var toursDuJour = await _chargementRepository.CountByZoneAndDateAsync(dto.ZoneId, heureDepart);
        if (toursDuJour >= zone.ToursMaxParJour)
            throw new InvalidOperationException(
                $"Le nombre maximum de tours par jour ({zone.ToursMaxParJour}) pour la zone {zone.Nom} est atteint.");

        var distanceAllerRetour = zone.Distance * 2;

        var chargement = new Chargement
        {
            CamionId = dto.CamionId,
            ZoneId = dto.ZoneId,
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
