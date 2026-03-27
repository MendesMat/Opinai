using Microsoft.EntityFrameworkCore;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Infrastructure.Persistence;

public class CrudRepositoryBase<T>(DbContext context) 
    : ICrudRepository<T> 
    where T : class, IEntity
{
    protected readonly DbSet<T> _set = context.Set<T>();

    public async Task AddAsync(T entity)
        => await _set.AddAsync(entity);

    public async Task<T?> GetByIdAsync(Guid id)
        => await _set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
        => await _set.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdWithTrackingAsync(Guid id)
        => await _set.FirstOrDefaultAsync(e => e.Id == id);

    public void Update(T entity)
        => _set.Update(entity);

    public void Delete(T entity)
        => _set.Remove(entity);
}
