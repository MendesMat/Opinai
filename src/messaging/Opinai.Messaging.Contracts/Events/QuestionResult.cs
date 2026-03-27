namespace Opinai.Messaging.Contracts.Events;

public record QuestionResult(
    int QuestionIndex,
    IReadOnlyCollection<AnswerResult> Answers
);
