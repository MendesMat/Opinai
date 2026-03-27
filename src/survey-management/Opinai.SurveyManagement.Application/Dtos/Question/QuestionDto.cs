using Opinai.SurveyManagement.Application.Dtos.Answer;

namespace Opinai.SurveyManagement.Application.Dtos.Question;

public record QuestionDto
(
    int Index,
    string Title,
    List<AnswerDto> Answers
);
