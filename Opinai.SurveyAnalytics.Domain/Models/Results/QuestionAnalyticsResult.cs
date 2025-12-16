namespace Opinai.SurveyAnalytics.Domain.Models.Results;

public record QuestionAnalyticsResult(
    int QuestionIndex,
    long TotalResponses,
    IReadOnlyCollection<AnswerAnalyticsResult> Answers
);
