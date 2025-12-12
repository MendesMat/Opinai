using Opinai.SurveyManagement.Application.Dtos.Answer;

namespace Opinai.SurveyManagement.Application.Dtos.Question;

public record CreateQuestionDto
(
    string Title,
    List<CreateAnswerDto> Answers
);