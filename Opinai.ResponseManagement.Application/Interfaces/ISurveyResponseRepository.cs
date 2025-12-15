using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Domain.Entities;

namespace Opinai.ResponseManagement.Application.Interfaces;

public interface ISurveyResponseRepository
{
    Task AddAsync(SurveyResponse entity);
    Task<IReadOnlyCollection<SurveyResponseAggregation>>
        GetAggregatedBySurveyAsync(Guid surveyId);
}