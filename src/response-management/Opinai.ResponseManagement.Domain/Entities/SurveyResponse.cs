namespace Opinai.ResponseManagement.Domain.Entities;

public class SurveyResponse
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid SurveyId { get; private set; }
    public int QuestionIndex { get; private set; }
    public int AnswerIndex { get; private set; }

    public SurveyResponse(
        Guid surveyId, 
        int questionIndex, 
        int answerIndex)
    {
        SurveyId = surveyId;
        QuestionIndex = questionIndex;
        AnswerIndex = answerIndex;
    }
}
