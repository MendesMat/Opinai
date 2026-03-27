using AutoMapper;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Application.Services;

public abstract class QueryServiceBase<T, TDto>
    (ICrudRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IQueryService<TDto> 
    where T : class, IEntity
{
    protected IMapper Mapper { get; } = mapper;
    protected ICrudRepository<T> Repository { get; } = repository;
    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public virtual async Task<TDto?> GetByIdAsync(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id).ConfigureAwait(false);
        if (entity is null) return default;

        var dto = Mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<IReadOnlyCollection<TDto>> GetAllAsync()
    {
        var entities = await Repository.GetAllAsync().ConfigureAwait(false);

        var dtos = Mapper.Map<IReadOnlyCollection<TDto>>(entities);
        return dtos;
    }
}
