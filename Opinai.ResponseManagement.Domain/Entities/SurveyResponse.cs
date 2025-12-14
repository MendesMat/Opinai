namespace Opinai.ResponseManagement.Domain.Entities;

internal class SurveyResponse(
    string surveyId, 
    int questionId, 
    int answerId)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SurveyId { get; set; } = surveyId;
    public int QuestionId { get; set; } = questionId;
    public int AnswerId { get; set; } = answerId;
}
