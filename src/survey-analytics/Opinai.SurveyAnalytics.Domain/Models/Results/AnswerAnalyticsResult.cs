namespace Opinai.SurveyAnalytics.Domain.Models.Results;

public record AnswerAnalyticsResult(
    int AnswerIndex,
    long Count,
    decimal Percentage
);
