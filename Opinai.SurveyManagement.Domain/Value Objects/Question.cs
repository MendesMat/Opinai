namespace Opinai.SurveyManagement.Domain;

public class Question(string title)
{
    public string Title { get; set; } = title;

    public List<Answer> Answers { get; set; } = [];
}