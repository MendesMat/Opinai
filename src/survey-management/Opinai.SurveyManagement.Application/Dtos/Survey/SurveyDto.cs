using Opinai.SurveyManagement.Application.Dtos.Question;
using Opinai.SurveyManagement.Domain.Enums;

namespace Opinai.SurveyManagement.Application.Dtos.Survey;

public record SurveyDto
(
    Guid Id,
    string Title,
    string Description,
    Status Status,
    List<QuestionDto> Questions    
);
