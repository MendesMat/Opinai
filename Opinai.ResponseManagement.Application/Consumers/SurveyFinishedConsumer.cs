using MassTransit;
using Opinai.Messaging.Contracts.Events;
using Opinai.ResponseManagement.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Consumers;

public class SurveyFinishedConsumer(
    ISurveyAvailabilityService surveyAvailabilityService,
    ISurveyResponseService surveyResponseService, 
    IPublishEndpoint publishEndpoint) 
    : IConsumer<SurveyFinished>
{
    public async Task Consume(ConsumeContext<SurveyFinished> context)
    {
        var surveyId = context.Message.SurveyId;
        surveyAvailabilityService.CloseSurvey(surveyId);

        var surveyResults = 
            await surveyResponseService.BuildSurveyResultsAsync(surveyId);

        var questions = surveyResults.Questions
        .Select(q => new QuestionResult(
            q.QuestionIndex,
            q.Answers.Select(a =>
                new AnswerResult(
                    a.AnswerIndex,
                    a.Count
                )
            ).ToList()
        )).ToList();

        await publishEndpoint.Publish(
            new SurveyResultsAggregated(surveyId, questions)
        );
    }
}
