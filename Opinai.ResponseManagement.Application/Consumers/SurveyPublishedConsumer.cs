using MassTransit;
using Opinai.Messaging.Contracts.Events;

namespace Opinai.ResponseManagement.Application.Consumers;

public class SurveyPublishedConsumer : IConsumer<SurveyPublished>
{
    public Task Consume(ConsumeContext<SurveyPublished> context)
    {
        var surveyId = context.Message.SurveyId;

        // Marcar survey como ativo em memória
        // Aquecer cache
        // Simplesmente aceitar (frontend já controla)

        return Task.CompletedTask;
    }
}
