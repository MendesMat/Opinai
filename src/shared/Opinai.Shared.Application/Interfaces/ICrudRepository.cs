namespace Opinai.Shared.Application.Interfaces;

public interface ICrudRepository<T> 
    where T : class, IEntity
{
    Task AddAsync(T entity);
    void Delete(T entity);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdWithTrackingAsync(Guid id);
    void Update(T entity);
}