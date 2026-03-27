using Opinai.Messaging.Contracts.Payloads;
using Opinai.SurveyAnalytics.Application.Interfaces;
using Opinai.SurveyAnalytics.Domain.Models.Inputs;
using Opinai.SurveyAnalytics.Domain.Models.Results;
using Opinai.SurveyAnalytics.Domain.Services;

namespace Opinai.SurveyAnalytics.Application.Services;

public class SurveyAnalyticsService : ISurveyAnalyticsService
{
    public Task<SurveyAnalyticsResult> BuildAsync(SurveyResultsPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload);

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

        return Task.FromResult(SurveyAnalyticsCalculator.Calculate(input));
    }
}
