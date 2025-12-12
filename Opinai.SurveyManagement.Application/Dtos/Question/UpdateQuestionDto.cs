using Opinai.SurveyManagement.Application.Dtos.Answer;

namespace Opinai.SurveyManagement.Application.Dtos.Question;

public record UpdateQuestionDto
(
    string? Title,
    List<UpdateAnswerDto> Answers
);