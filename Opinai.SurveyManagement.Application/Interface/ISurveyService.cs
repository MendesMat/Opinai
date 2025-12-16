using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Enums;

namespace Opinai.SurveyManagement.Application.Interface;

public interface ISurveyService : IQueryService<SurveyDto>
{
    Task<Guid> CreateAsync(CreateSurveyDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateSurveyDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<SurveyActionResult> PublishSurveyAsync(Guid id);
    Task<SurveyActionResult> FinishSurveyAsync(Guid id);
}
