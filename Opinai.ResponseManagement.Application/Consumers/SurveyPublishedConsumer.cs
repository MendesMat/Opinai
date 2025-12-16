using MassTransit;
using Opinai.Messaging.Contracts.Events;
using Opinai.ResponseManagement.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Consumers;

public class SurveyPublishedConsumer(ISurveyAvailabilityService surveyAvailabilityService) : 
    IConsumer<SurveyPublished>
{
    public Task Consume(ConsumeContext<SurveyPublished> context)
    {
        var surveyId = context.Message.SurveyId;
        surveyAvailabilityService.OpenSurvey(surveyId);

        return Task.CompletedTask;
    }
}
