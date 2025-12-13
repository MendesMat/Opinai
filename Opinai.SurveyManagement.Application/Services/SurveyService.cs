using AutoMapper;
using Opinai.Shared.Application.Interfaces;
using Opinai.Shared.Application.Services;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Interface;
using Opinai.SurveyManagement.Domain;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Application.Services;

public class SurveyService(ICrudRepository<Survey> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : CrudServiceBase<Survey, SurveyDto, CreateSurveyDto, UpdateSurveyDto>(repository, unitOfWork, mapper),
    ISurveyService
{
    public override async Task<bool> UpdateAsync(Guid id, UpdateSurveyDto dto)
    {
        var entity = await _repository.GetByIdForUpdateAsync(id);
        if (entity is null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Title))
            entity.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            entity.Description = dto.Description;

        if (dto.Status.HasValue)
            entity.Status = dto.Status.Value;

        entity.Questions = dto.Questions
            .Select(q => _mapper.Map<Question>(q))
            .ToList();

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
