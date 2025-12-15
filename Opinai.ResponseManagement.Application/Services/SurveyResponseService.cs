using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Domain.Entities;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyResponseService(
    ISurveyResponseRepository repository,
    IUnitOfWork unitOfWork) : ISurveyResponseService
{
    public async Task AddSurveyResponseAsync(SurveyResponseDto dto)
    {
        foreach (var answer in dto.Answers)
        {
            var response = new SurveyResponse(
                dto.SurveyId,
                answer.QuestionIndex,
                answer.AnswerIndex
            );

            await repository.AddAsync(response);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
