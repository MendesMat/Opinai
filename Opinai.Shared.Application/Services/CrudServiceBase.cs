using AutoMapper;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Application.Services;

public abstract class CrudServiceBase<T, TDto, TCreateDto, TUpdateDto>
    (ICrudRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : ICrudService<TDto, TCreateDto, TUpdateDto> 
    where T : class, IEntity
{
    protected readonly ICrudRepository<T> _repository = repository;
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IMapper _mapper = mapper;

    public virtual async Task<Guid> CreateAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<T>(dto);

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.Id;
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return default;

        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<IReadOnlyCollection<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();

        var dtos = _mapper.Map<IReadOnlyCollection<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<bool> UpdateAsync(Guid id, TUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return false;

        _mapper.Map(dto, entity);

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
