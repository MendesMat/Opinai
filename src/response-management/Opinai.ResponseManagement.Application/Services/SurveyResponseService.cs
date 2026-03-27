using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Application.Enums;
using Opinai.Messaging.Contracts.Payloads;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Domain.Entities;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyResponseService(
    //ISurveyAvailabilityService availabilityService,
    ISurveyResponseRepository repository,
    IUnitOfWork unitOfWork) : ISurveyResponseService
{
    public async Task<SurveyResponseResult> AddSurveyResponseAsync(SurveyResponseDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (dto.SurveyId == Guid.Empty)
            throw new ArgumentException("SurveyId não pode ser vazio.");

        ArgumentNullException.ThrowIfNull(dto.Answers);
        if (!dto.Answers.Any())
            throw new ArgumentException("A lista de respostas não pode ser vazia.");

        // O código abaixo seria usado apenas se a mensageria não fosse In Memory
        // No entanto, deixei a mensageria toda configurada para fins de avaliação.

        //if (!availabilityService.IsSurveyPublished(dto.SurveyId))
        //    return SurveyResponseResult.SurveyNotPublished;

        foreach (var answer in dto.Answers)
        {
            var response = new SurveyResponse(
                dto.SurveyId,
                answer.QuestionIndex,
                answer.AnswerIndex
            );

            await repository.AddRangeAsync(response).ConfigureAwait(false);
        }

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        return SurveyResponseResult.Success;
    }

    public async Task<SurveyResultsPayload> BuildSurveyResultsAsync(Guid surveyId)
    {
        if (surveyId == Guid.Empty)
            throw new ArgumentException("SurveyId não pode ser vazio.");

        var aggregation = await repository.GetAggregatedBySurveyAsync(surveyId).ConfigureAwait(false);

        var questions = aggregation.GroupBy(r => r.QuestionIndex)
            .Select(q => new QuestionResultsPayload(
                q.Key,
                q.Select(a => new AnswerResultsPayload(
                    a.AnswerIndex,
                    a.Count
                )).ToList()
            )).ToList();

        return new SurveyResultsPayload(surveyId, questions);
    }
}
