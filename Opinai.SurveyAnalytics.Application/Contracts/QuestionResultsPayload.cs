namespace Opinai.SurveyAnalytics.Application.Contracts;

public record QuestionResultsPayload(
    int QuestionIndex,
    IReadOnlyCollection<AnswerResultsPayload> Answers
);
