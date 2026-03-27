namespace Opinai.Messaging.Contracts.Payloads;

public record AnswerResultsPayload(
    int AnswerIndex,
    long Count
);
