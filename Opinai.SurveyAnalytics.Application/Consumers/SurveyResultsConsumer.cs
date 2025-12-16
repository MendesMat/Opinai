using MassTransit;
using Opinai.Messaging.Contracts.Events;
using Opinai.SurveyAnalytics.Application.Contracts;
using Opinai.SurveyAnalytics.Application.Interfaces;

namespace Opinai.SurveyAnalytics.Application.Consumers;

public class SurveyResultsConsumer(ISurveyAnalyticsService service)
    : IConsumer<SurveyResultsAggregated>
{
    public async Task Consume(
        ConsumeContext<SurveyResultsAggregated> context)
    {
        var message = context.Message;

        var payload = new SurveyResultsPayload(
            message.SurveyId,
            
            message.Questions.Select(q =>
                new QuestionResultsPayload(
                    q.QuestionIndex,
                    
                    q.Answers.Select(a =>
                        new AnswerResultsPayload(
                            a.AnswerIndex,
                            a.Count
                        )
                    ).ToList()
                )
            ).ToList()
        );

        await service.BuildAsync(payload);
    }
}
