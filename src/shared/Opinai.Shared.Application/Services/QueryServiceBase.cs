using AutoMapper;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Application.Services;

public abstract class QueryServiceBase<T, TDto>
    (ICrudRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IQueryService<TDto> 
    where T : class, IEntity
{
    protected readonly IMapper _mapper = mapper;
    protected readonly ICrudRepository<T> _repository = repository;
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

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
}
