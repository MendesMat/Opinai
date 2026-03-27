namespace Opinai.SurveyAnalytics.Domain.Models.Results;

public record SurveyAnalyticsResult(
    Guid SurveyId,
    IReadOnlyCollection<QuestionAnalyticsResult> Questions
);
