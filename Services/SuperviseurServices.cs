using AutoMapper;
using TCA.API.DTOs;
using TCA.API.Models;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface ISuperviseurGeneralService
{
    Task<IEnumerable<SuperviseurGeneralDto>> GetAllAsync();
    Task<SuperviseurGeneralDto?> GetByIdAsync(int id);
    Task<SuperviseurGeneralDto> CreateAsync(CreateSuperviseurGeneralDto dto);
    Task<SuperviseurGeneralDto?> UpdateAsync(int id, UpdateSuperviseurGeneralDto dto);
    Task<bool> DeleteAsync(int id);
}

public class SuperviseurGeneralService : ISuperviseurGeneralService
{
    private readonly ISuperviseurGeneralRepository _repository;
    private readonly IMapper _mapper;

    public SuperviseurGeneralService(ISuperviseurGeneralRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SuperviseurGeneralDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SuperviseurGeneralDto>>(entities);
    }

    public async Task<SuperviseurGeneralDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : _mapper.Map<SuperviseurGeneralDto>(entity);
    }

    public async Task<SuperviseurGeneralDto> CreateAsync(CreateSuperviseurGeneralDto dto)
    {
        var entity = _mapper.Map<SuperviseurGeneral>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SuperviseurGeneralDto>(entity);
    }

    public async Task<SuperviseurGeneralDto?> UpdateAsync(int id, UpdateSuperviseurGeneralDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SuperviseurGeneralDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
        return true;
    }
}

public interface ISuperviseurZoneService
{
    Task<IEnumerable<SuperviseurZoneDto>> GetAllAsync();
    Task<SuperviseurZoneDto?> GetByIdAsync(int id);
    Task<SuperviseurZoneDto> CreateAsync(CreateSuperviseurZoneDto dto);
    Task<SuperviseurZoneDto?> UpdateAsync(int id, UpdateSuperviseurZoneDto dto);
    Task<bool> DeleteAsync(int id);
}

public class SuperviseurZoneService : ISuperviseurZoneService
{
    private readonly ISuperviseurZoneRepository _repository;
    private readonly IMapper _mapper;

    public SuperviseurZoneService(ISuperviseurZoneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SuperviseurZoneDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllWithZoneAsync();
        return _mapper.Map<IEnumerable<SuperviseurZoneDto>>(entities);
    }

    public async Task<SuperviseurZoneDto?> GetByIdAsync(int id)
    {
        var entities = await _repository.GetAllWithZoneAsync();
        var entity = entities.FirstOrDefault(s => s.Id == id);
        return entity is null ? null : _mapper.Map<SuperviseurZoneDto>(entity);
    }

    public async Task<SuperviseurZoneDto> CreateAsync(CreateSuperviseurZoneDto dto)
    {
        var entity = _mapper.Map<SuperviseurZone>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return (await GetByIdAsync(entity.Id))!;
    }

    public async Task<SuperviseurZoneDto?> UpdateAsync(int id, UpdateSuperviseurZoneDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
        return true;
    }
}

public interface ISuperviseurGroupeService
{
    Task<IEnumerable<SuperviseurGroupeDto>> GetAllAsync();
    Task<SuperviseurGroupeDto?> GetByIdAsync(int id);
    Task<SuperviseurGroupeDto> CreateAsync(CreateSuperviseurGroupeDto dto);
    Task<SuperviseurGroupeDto?> UpdateAsync(int id, UpdateSuperviseurGroupeDto dto);
    Task<bool> DeleteAsync(int id);
}

public class SuperviseurGroupeService : ISuperviseurGroupeService
{
    private readonly ISuperviseurGroupeRepository _repository;
    private readonly IMapper _mapper;

    public SuperviseurGroupeService(ISuperviseurGroupeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SuperviseurGroupeDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllWithGroupeAsync();
        return _mapper.Map<IEnumerable<SuperviseurGroupeDto>>(entities);
    }

    public async Task<SuperviseurGroupeDto?> GetByIdAsync(int id)
    {
        var entities = await _repository.GetAllWithGroupeAsync();
        var entity = entities.FirstOrDefault(s => s.Id == id);
        return entity is null ? null : _mapper.Map<SuperviseurGroupeDto>(entity);
    }

    public async Task<SuperviseurGroupeDto> CreateAsync(CreateSuperviseurGroupeDto dto)
    {
        var entity = _mapper.Map<SuperviseurGroupe>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return (await GetByIdAsync(entity.Id))!;
    }

    public async Task<SuperviseurGroupeDto?> UpdateAsync(int id, UpdateSuperviseurGroupeDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
        return true;
    }
}
