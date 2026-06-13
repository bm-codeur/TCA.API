using AutoMapper;
using TCA.API.DTOs;
using TCA.API.Models;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface IZoneService
{
    Task<IEnumerable<ZoneDto>> GetAllAsync();
    Task<ZoneDto?> GetByIdAsync(int id);
    Task<ZoneDto> CreateAsync(CreateZoneDto dto);
    Task<ZoneDto?> UpdateAsync(int id, UpdateZoneDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ZoneService : IZoneService
{
    private readonly IZoneRepository _repository;
    private readonly IMapper _mapper;

    public ZoneService(IZoneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ZoneDto>> GetAllAsync()
    {
        var zones = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ZoneDto>>(zones);
    }

    public async Task<ZoneDto?> GetByIdAsync(int id)
    {
        var zone = await _repository.GetByIdAsync(id);
        return zone is null ? null : _mapper.Map<ZoneDto>(zone);
    }

    public async Task<ZoneDto> CreateAsync(CreateZoneDto dto)
    {
        var zone = _mapper.Map<Zone>(dto);
        await _repository.AddAsync(zone);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ZoneDto>(zone);
    }

    public async Task<ZoneDto?> UpdateAsync(int id, UpdateZoneDto dto)
    {
        var zone = await _repository.GetByIdAsync(id);
        if (zone is null) return null;

        _mapper.Map(dto, zone);
        _repository.Update(zone);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ZoneDto>(zone);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var zone = await _repository.GetByIdAsync(id);
        if (zone is null) return false;

        _repository.Delete(zone);
        await _repository.SaveChangesAsync();
        return true;
    }
}

public interface IGroupeService
{
    Task<IEnumerable<GroupeDto>> GetAllAsync();
    Task<GroupeDto?> GetByIdAsync(int id);
    Task<GroupeDto> CreateAsync(CreateGroupeDto dto);
    Task<GroupeDto?> UpdateAsync(int id, UpdateGroupeDto dto);
    Task<bool> DeleteAsync(int id);
}

public class GroupeService : IGroupeService
{
    private readonly IGroupeRepository _repository;
    private readonly IMapper _mapper;

    public GroupeService(IGroupeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupeDto>> GetAllAsync()
    {
        var groupes = await _repository.GetAllWithZoneAsync();
        return _mapper.Map<IEnumerable<GroupeDto>>(groupes);
    }

    public async Task<GroupeDto?> GetByIdAsync(int id)
    {
        var groupe = await _repository.GetByIdWithZoneAsync(id);
        return groupe is null ? null : _mapper.Map<GroupeDto>(groupe);
    }

    public async Task<GroupeDto> CreateAsync(CreateGroupeDto dto)
    {
        var groupe = _mapper.Map<Groupe>(dto);
        await _repository.AddAsync(groupe);
        await _repository.SaveChangesAsync();
        return (await GetByIdAsync(groupe.Id))!;
    }

    public async Task<GroupeDto?> UpdateAsync(int id, UpdateGroupeDto dto)
    {
        var groupe = await _repository.GetByIdAsync(id);
        if (groupe is null) return null;

        _mapper.Map(dto, groupe);
        _repository.Update(groupe);
        await _repository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var groupe = await _repository.GetByIdAsync(id);
        if (groupe is null) return false;

        _repository.Delete(groupe);
        await _repository.SaveChangesAsync();
        return true;
    }
}

public interface ICamionService
{
    Task<IEnumerable<CamionDto>> GetAllAsync();
    Task<CamionDto?> GetByIdAsync(int id);
    Task<CamionDto> CreateAsync(CreateCamionDto dto);
    Task<CamionDto?> UpdateAsync(int id, UpdateCamionDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CamionService : ICamionService
{
    private readonly ICamionRepository _repository;
    private readonly IMapper _mapper;

    public CamionService(ICamionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CamionDto>> GetAllAsync()
    {
        var camions = await _repository.GetAllWithGroupeAsync();
        return _mapper.Map<IEnumerable<CamionDto>>(camions);
    }

    public async Task<CamionDto?> GetByIdAsync(int id)
    {
        var camion = await _repository.GetByIdWithGroupeAsync(id);
        return camion is null ? null : _mapper.Map<CamionDto>(camion);
    }

    public async Task<CamionDto> CreateAsync(CreateCamionDto dto)
    {
        var camion = _mapper.Map<Camion>(dto);
        await _repository.AddAsync(camion);
        await _repository.SaveChangesAsync();
        return (await GetByIdAsync(camion.Id))!;
    }

    public async Task<CamionDto?> UpdateAsync(int id, UpdateCamionDto dto)
    {
        var camion = await _repository.GetByIdAsync(id);
        if (camion is null) return null;

        _mapper.Map(dto, camion);
        _repository.Update(camion);
        await _repository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var camion = await _repository.GetByIdAsync(id);
        if (camion is null) return false;

        _repository.Delete(camion);
        await _repository.SaveChangesAsync();
        return true;
    }
}

public interface IChauffeurService
{
    Task<IEnumerable<ChauffeurDto>> GetAllAsync();
    Task<ChauffeurDto?> GetByIdAsync(int id);
    Task<ChauffeurDto> CreateAsync(CreateChauffeurDto dto);
    Task<ChauffeurDto?> UpdateAsync(int id, UpdateChauffeurDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ChauffeurService : IChauffeurService
{
    private readonly IChauffeurRepository _repository;
    private readonly IMapper _mapper;

    public ChauffeurService(IChauffeurRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChauffeurDto>> GetAllAsync()
    {
        var chauffeurs = await _repository.GetAllWithCamionAsync();
        return _mapper.Map<IEnumerable<ChauffeurDto>>(chauffeurs);
    }

    public async Task<ChauffeurDto?> GetByIdAsync(int id)
    {
        var chauffeur = await _repository.GetByIdWithCamionAsync(id);
        return chauffeur is null ? null : _mapper.Map<ChauffeurDto>(chauffeur);
    }

    public async Task<ChauffeurDto> CreateAsync(CreateChauffeurDto dto)
    {
        var chauffeur = _mapper.Map<Chauffeur>(dto);
        await _repository.AddAsync(chauffeur);
        await _repository.SaveChangesAsync();
        return (await GetByIdAsync(chauffeur.Id))!;
    }

    public async Task<ChauffeurDto?> UpdateAsync(int id, UpdateChauffeurDto dto)
    {
        var chauffeur = await _repository.GetByIdAsync(id);
        if (chauffeur is null) return null;

        _mapper.Map(dto, chauffeur);
        _repository.Update(chauffeur);
        await _repository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var chauffeur = await _repository.GetByIdAsync(id);
        if (chauffeur is null) return false;

        _repository.Delete(chauffeur);
        await _repository.SaveChangesAsync();
        return true;
    }
}
