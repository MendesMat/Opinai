using Opinai.SurveyManagement.Application.Dtos.Question;
namespace Opinai.SurveyManagement.Application.Dtos.Survey;

public record CreateSurveyDto
(
    string Title,
    string Description,
    List<QuestionDto> Questions
);
