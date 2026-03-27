namespace Opinai.SurveyAnalytics.Domain.Models.Inputs;

public record QuestionAnalyticsInput(
    int QuestionIndex,
    IReadOnlyCollection<AnswerAnalyticsInput> Answers
);
