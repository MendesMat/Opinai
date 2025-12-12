namespace Opinai.Shared.Application.Interfaces;

public interface ICrudService<TDto, TCreateDto, TUpdateDto>
{
    Task<Guid> CreateAsync(TCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IReadOnlyCollection<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, TUpdateDto dto);
}