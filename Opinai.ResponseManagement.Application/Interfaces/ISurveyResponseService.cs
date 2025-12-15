namespace Opinai.ResponseManagement.Application.Interfaces
{
    public interface ISurveyResponseService
    {
        Task AddSurveyResponseAsync(Guid surveyId, int questionIndex, int answerIndex);
    }
}