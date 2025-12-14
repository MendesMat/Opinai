using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Application.Dtos.Survey;

namespace Opinai.SurveyManagement.Application.Interface;

public interface ISurveyService : IQueryService<SurveyDto>
{
    Task<Guid> CreateAsync(CreateSurveyDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateSurveyDto dto);
    Task<bool> DeleteAsync(Guid id);
}
