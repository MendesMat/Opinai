using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Application.Enums;
using Opinai.ResponseManagement.Application.Integration;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Domain.Entities;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyResponseService(
    ISurveyAvailabilityService availabilityService,
    ISurveyResponseRepository repository,
    IUnitOfWork unitOfWork) : ISurveyResponseService
{
    public async Task<SurveyResponseResult> AddSurveyResponseAsync(SurveyResponseDto dto)
    {
        if (!availabilityService.IsSurveyPublished(dto.SurveyId))
            return SurveyResponseResult.SurveyNotPublished;

        foreach (var answer in dto.Answers)
        {
            var response = new SurveyResponse(
                dto.SurveyId,
                answer.QuestionIndex,
                answer.AnswerIndex
            );

            await repository.AddRangeAsync(response);
        }

        await unitOfWork.SaveChangesAsync();
        return SurveyResponseResult.Success;
    }

    public async Task<SurveyResultsPayload> BuildSurveyResultsAsync(Guid surveyId)
    {
        var aggregation = await repository.GetAggregatedBySurveyAsync(surveyId);

        var questions = aggregation.GroupBy(r => r.QuestionIndex)
            .Select(q => new QuestionResultsPayload(
                q.Key,
                q.Select( a => new AnswerResultsPayload(
                    a.AnswerIndex,
                    a.Count
                )).ToList()
            )).ToList();

        return new SurveyResultsPayload(surveyId, questions);
    }
}
