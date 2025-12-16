using Opinai.ResponseManagement.Application.Interfaces;
using System.Collections.Concurrent;

namespace Opinai.ResponseManagement.Application.Services;

public class SurveyAvailabilityService : ISurveyAvailabilityService
{
    private readonly ConcurrentDictionary<Guid, bool> _surveys = new();

    public void OpenSurvey(Guid surveyId)
        => _surveys[surveyId] = true;

    public void CloseSurvey(Guid surveyId)
        => _surveys[surveyId] = false;

    public bool IsSurveyOpen(Guid surveyId)
    {
        if (!_surveys.TryGetValue(surveyId, out var isOpen))
            return false;

        return isOpen;
    }
}
