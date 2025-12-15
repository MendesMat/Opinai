using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Domain.Entities;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyResponseService(
    ISurveyResponseRepository repository,
    IUnitOfWork unitOfWork) : ISurveyResponseService
{
    public async Task AddSurveyResponseAsync(
        Guid surveyId,
        int questionIndex,
        int answerIndex)
    {
        var response = new SurveyResponse(
            surveyId,
            questionIndex,
            answerIndex
        );

        await repository.AddAsync(response);
        await unitOfWork.SaveChangesAsync();
    }
}
