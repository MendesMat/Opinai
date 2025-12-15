namespace Opinai.ResponseManagement.Application.Dtos;

public record SurveyResponseDto(
    Guid SurveyId,
    IReadOnlyCollection<QuestionAnswerDto> Answers
);