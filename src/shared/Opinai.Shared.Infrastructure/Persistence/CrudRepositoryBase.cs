using Microsoft.EntityFrameworkCore;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Infrastructure.Persistence;

public class CrudRepositoryBase<T>(DbContext context) 
    : ICrudRepository<T> 
    where T : class, IEntity
{
    protected DbSet<T> Set { get; } = context.Set<T>();

    public async Task AddAsync(T entity)
        => await Set.AddAsync(entity).ConfigureAwait(false);

    public async Task<T?> GetByIdAsync(Guid id)
        => await Set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
        => await Set.AsNoTracking().ToListAsync().ConfigureAwait(false);

    public async Task<T?> GetByIdWithTrackingAsync(Guid id)
        => await Set.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

    public void Update(T entity)
        => Set.Update(entity);

    public void Delete(T entity)
        => Set.Remove(entity);
}
