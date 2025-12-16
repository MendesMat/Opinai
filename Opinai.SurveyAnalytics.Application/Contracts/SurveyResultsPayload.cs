namespace Opinai.SurveyAnalytics.Application.Contracts;

public record SurveyResultsPayload(
    Guid SurveyId,
    IReadOnlyCollection<QuestionResultsPayload> Questions
);
