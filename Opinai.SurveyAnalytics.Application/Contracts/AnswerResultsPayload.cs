namespace Opinai.SurveyAnalytics.Application.Contracts;

public record AnswerResultsPayload(
    int AnswerIndex,
    long Count
);
