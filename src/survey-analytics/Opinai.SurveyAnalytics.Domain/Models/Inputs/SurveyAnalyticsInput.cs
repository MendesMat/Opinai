namespace Opinai.SurveyAnalytics.Domain.Models.Inputs;

public record SurveyAnalyticsInput(
    Guid SurveyId,
    IReadOnlyCollection<QuestionAnalyticsInput> Questions
);
