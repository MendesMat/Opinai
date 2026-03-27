namespace Opinai.Messaging.Contracts.Payloads;

public record SurveyResultsPayload(
    Guid SurveyId,
    IReadOnlyCollection<QuestionResultsPayload> Questions
);
