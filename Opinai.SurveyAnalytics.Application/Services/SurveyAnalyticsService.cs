using Opinai.SurveyAnalytics.Application.Contracts;
using Opinai.SurveyAnalytics.Application.Interfaces;
using Opinai.SurveyAnalytics.Domain.Models.Inputs;
using Opinai.SurveyAnalytics.Domain.Models.Results;
using Opinai.SurveyAnalytics.Domain.Services;

namespace Opinai.SurveyAnalytics.Application.Services;

public class SurveyAnalyticsService(
    SurveyAnalyticsCalculator calculator) : ISurveyAnalyticsService
{
    private readonly SurveyAnalyticsCalculator _calculator = calculator;

    public Task<SurveyAnalyticsResult> BuildAsync(SurveyResultsPayload payload)
    {
        var input = new SurveyAnalyticsInput(
            payload.SurveyId,
            payload.Questions.Select(q =>
                new QuestionAnalyticsInput(
                    q.QuestionIndex,
                    q.Answers.Select(a =>
                        new AnswerAnalyticsInput(
                            a.AnswerIndex,
                            a.Count
                        )
                    ).ToList()
                )
            ).ToList()
        );

        return Task.FromResult(_calculator.Calculate(input));
    }
}
