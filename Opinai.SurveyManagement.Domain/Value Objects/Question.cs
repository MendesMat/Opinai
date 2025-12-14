namespace Opinai.SurveyManagement.Domain;

public class Question
{
    public int Index { get; } = -1;
    public string Title { get; }

    public IReadOnlyCollection<Answer> Answers { get; }

    public Question(string title, IEnumerable<Answer> answers)
    {
        Index = -1;
        Title = title;
        Answers = answers.ToList();
    }

    private Question(int index, string title, IReadOnlyCollection<Answer> answers)
    {
        Index = index;
        Title = title;
        
        Answers = answers.Select((answer, index) 
            => answer.WithIndex(index)).ToList();
    }

    public Question WithIndex(int index)
        => new(Index, Title, Answers);
}