using Opinai.ResponseManagement.Application.ReadModels;
using Opinai.ResponseManagement.Domain.Entities;

namespace Opinai.ResponseManagement.Application.Interfaces;

public interface ISurveyResponseRepository
{
    Task AddRangeAsync(SurveyResponse entity);
    Task<IReadOnlyCollection<SurveyResponseAggregation>>
        GetAggregatedBySurveyAsync(Guid surveyId);
}