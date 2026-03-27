namespace Opinai.Messaging.Contracts.Payloads;

public record QuestionResultsPayload(
    int QuestionIndex,
    IReadOnlyCollection<AnswerResultsPayload> Answers
);
