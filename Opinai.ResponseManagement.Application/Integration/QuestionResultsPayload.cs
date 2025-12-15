namespace Opinai.ResponseManagement.Application.Integration;

public record QuestionResultsPayload(
    int QuestionIndex,
    IReadOnlyCollection<AnswerResultsPayload> Answers
);
