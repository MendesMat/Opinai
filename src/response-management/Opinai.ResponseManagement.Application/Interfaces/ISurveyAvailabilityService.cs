namespace Opinai.ResponseManagement.Application.Interfaces;

public interface ISurveyAvailabilityService
{
    void CloseSurvey(Guid surveyId);
    bool IsSurveyPublished(Guid surveyId);
    void OpenSurvey(Guid surveyId);
}