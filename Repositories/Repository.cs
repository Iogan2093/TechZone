using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TechZone.Data;

namespace TechZone.Repositories;

// Базовая реализация универсального репозитория поверх EF Core
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly TechZoneDbContext _devices;
    protected readonly DbSet<T> _set;

    public Repository(TechZoneDbContext context)
    {
        _devices = context;
        _set = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

    // Поиск по произвольному условию
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _set.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);

    public async Task<int> SaveChangesAsync() => await _devices.SaveChangesAsync();
}
