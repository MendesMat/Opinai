using Opinai.ResponseManagement.Application.Interfaces;
using System.Collections.Concurrent;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyAvailabilityService : ISurveyAvailabilityService
{
    private readonly ConcurrentDictionary<Guid, byte> _publishedSurveys = new();

    public void OpenSurvey(Guid surveyId)
        => _publishedSurveys.TryAdd(surveyId, 0);

    public void CloseSurvey(Guid surveyId)
        => _publishedSurveys.TryRemove(surveyId, out _);

    public bool IsSurveyPublished(Guid surveyId) 
        => _publishedSurveys.ContainsKey(surveyId);
}
