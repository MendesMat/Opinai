using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Application.Dtos.Survey;

namespace Opinai.SurveyManagement.Application.Interface;

public interface ISurveyService :
    ICrudService<SurveyDto, CreateSurveyDto, UpdateSurveyDto>
{
}
