namespace Opinai.ResponseManagement.Application.Dtos;

public record SurveyResponseAggregation(
    int QuestionIndex,
    int AnswerIndex,
    long Count
);
