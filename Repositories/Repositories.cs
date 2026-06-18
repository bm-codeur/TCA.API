using Microsoft.EntityFrameworkCore;
using TCA.API.Data;
using TCA.API.Models;
using TCA.API.Repositories.Interfaces;

namespace TCA.API.Repositories;

public interface IZoneRepository : IRepository<Zone>
{
    Task<Zone?> GetByNomAsync(string nom);
}

public class ZoneRepository : RepositoryRepository<Zone>, IZoneRepository
{
    public ZoneRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Zone?> GetByNomAsync(string nom) =>
        await _dbSet.FirstOrDefaultAsync(z => z.Nom == nom);
}

public interface IGroupeRepository : IRepository<Groupe>
{
    Task<Groupe?> GetByIdWithZoneAsync(int id);
    Task<IEnumerable<Groupe>> GetAllWithZoneAsync();
}

public class GroupeRepository : RepositoryRepository<Groupe>, IGroupeRepository
{
    public GroupeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Groupe?> GetByIdWithZoneAsync(int id) =>
        await _dbSet.Include(g => g.Zone).FirstOrDefaultAsync(g => g.Id == id);

    public async Task<IEnumerable<Groupe>> GetAllWithZoneAsync() =>
        await _dbSet.Include(g => g.Zone).ToListAsync();
}

public interface ICamionRepository : IRepository<Camion>
{
    Task<Camion?> GetByIdWithGroupeAsync(int id);
    Task<IEnumerable<Camion>> GetAllWithGroupeAsync();
    Task<Camion?> GetByIdWithChargementsAsync(int id);
}

public class CamionRepository : RepositoryRepository<Camion>, ICamionRepository
{
    public CamionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Camion?> GetByIdWithGroupeAsync(int id) =>
        await _dbSet.Include(c => c.Groupe).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Camion>> GetAllWithGroupeAsync() =>
        await _dbSet.Include(c => c.Groupe).ToListAsync();

    public async Task<Camion?> GetByIdWithChargementsAsync(int id) =>
    await _dbSet
        .Include(c => c.Chargements)
        .Include(c => c.Groupe)
            .ThenInclude(g => g!.Zone)
        .FirstOrDefaultAsync(c => c.Id == id);
}

public interface IChauffeurRepository : IRepository<Chauffeur>
{
    Task<Chauffeur?> GetByIdWithCamionAsync(int id);
    Task<IEnumerable<Chauffeur>> GetAllWithCamionAsync();
}

public class ChauffeurRepository : RepositoryRepository<Chauffeur>, IChauffeurRepository
{
    public ChauffeurRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Chauffeur?> GetByIdWithCamionAsync(int id) =>
        await _dbSet.Include(c => c.Camion).ThenInclude(c => c!.Groupe).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Chauffeur>> GetAllWithCamionAsync() =>
        await _dbSet.Include(c => c.Camion).ToListAsync();
}

public interface IChargementRepository : IRepository<Chargement>
{
    Task<Chargement?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Chargement>> GetAllWithDetailsAsync();
    Task<int> CountByZoneAndDateAsync(int zoneId, DateTime date);
    Task<Chargement?> GetDernierChargementCamionAsync(int camionId);
    Task<IEnumerable<Chargement>> GetByDateRangeAsync(DateTime start, DateTime end);
}

public class ChargementRepository : RepositoryRepository<Chargement>, IChargementRepository
{
    public ChargementRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Chargement?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet
            .Include(c => c.Camion)
            .Include(c => c.Zone)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Chargement>> GetAllWithDetailsAsync() =>
        await _dbSet
            .Include(c => c.Camion)
            .Include(c => c.Zone)
            .ToListAsync();

    public async Task<int> CountByZoneAndDateAsync(int zoneId, DateTime date) =>
        await _dbSet.CountAsync(c =>
            c.ZoneId == zoneId &&
            c.DateChargement.Date == date.Date);

    public async Task<Chargement?> GetDernierChargementCamionAsync(int camionId) =>
        await _dbSet
            .Where(c => c.CamionId == camionId)
            .OrderByDescending(c => c.HeureDepart)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Chargement>> GetByDateRangeAsync(DateTime start, DateTime end) =>
        await _dbSet
            .Include(c => c.Camion).ThenInclude(c => c!.Groupe)
            .Include(c => c.Zone)
            .Where(c => c.DateChargement >= start && c.DateChargement <= end)
            .ToListAsync();
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}

public class UserRepository : RepositoryRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
}

public interface ISuperviseurGroupeRepository : IRepository<SuperviseurGroupe>
{
    Task<IEnumerable<SuperviseurGroupe>> GetAllWithGroupeAsync();
}

public class SuperviseurGroupeRepository : RepositoryRepository<SuperviseurGroupe>, ISuperviseurGroupeRepository
{
    public SuperviseurGroupeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SuperviseurGroupe>> GetAllWithGroupeAsync() =>
        await _dbSet.Include(s => s.Groupe).ThenInclude(g => g!.Zone).ToListAsync();
}

public interface ISuperviseurZoneRepository : IRepository<SuperviseurZone>
{
    Task<IEnumerable<SuperviseurZone>> GetAllWithZoneAsync();
}

public class SuperviseurZoneRepository : RepositoryRepository<SuperviseurZone>, ISuperviseurZoneRepository
{
    public SuperviseurZoneRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SuperviseurZone>> GetAllWithZoneAsync() =>
        await _dbSet.Include(s => s.Zone).ToListAsync();
}

public interface ISuperviseurGeneralRepository : IRepository<SuperviseurGeneral>
{
}

public class SuperviseurGeneralRepository : RepositoryRepository<SuperviseurGeneral>, ISuperviseurGeneralRepository
{
    public SuperviseurGeneralRepository(ApplicationDbContext context) : base(context)
    {
    }
}
