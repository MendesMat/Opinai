namespace Opinai.Messaging.Contracts.Events;

public record SurveyResultsAggregated(
    Guid SurveyId,
    IReadOnlyCollection<QuestionResult> Questions
);
