namespace Opinai.ResponseManagement.Application.Dtos;

public record SurveyResponseDto(
    Guid SurveyId,
    int QuestionId,
    int AnswerId
);