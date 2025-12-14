namespace Opinai.Shared.Application.Interfaces;

public interface IQueryService<TDto>
{
    Task<TDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<TDto>> GetAllAsync();
}