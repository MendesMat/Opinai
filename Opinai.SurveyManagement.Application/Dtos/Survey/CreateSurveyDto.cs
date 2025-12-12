using Opinai.SurveyManagement.Application.Dtos.Question;
using Opinai.SurveyManagement.Domain.Enums;

namespace Opinai.SurveyManagement.Application.Dtos.Survey;

public record CreateSurveyDto
(
    string Title,
    string Description,
    List<CreateQuestionDto> Questions
);
