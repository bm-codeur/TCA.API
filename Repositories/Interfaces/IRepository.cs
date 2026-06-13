using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TCA.API.Data;

namespace TCA.API.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}

public class RepositoryRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public virtual void Update(T entity) =>
        _dbSet.Update(entity);

    public virtual void Delete(T entity) =>
        _dbSet.Remove(entity);

    public virtual async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
