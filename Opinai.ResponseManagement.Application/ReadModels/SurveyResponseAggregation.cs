namespace Opinai.ResponseManagement.Application.ReadModels;

public record SurveyResponseAggregation(
    int QuestionIndex,
    int AnswerIndex,
    long Count
);
