namespace Opinai.SurveyManagement.Domain;

public class Question
{
    public int Index { get; private set; } = -1;
    public string Title { get; private set; } = string.Empty;

    private readonly List<Answer> _answers = [];
    public IReadOnlyCollection<Answer> Answers => _answers;

    public Question() { }

    public Question(string title, IEnumerable<Answer> answers)
    {
        Title = title;
        _answers.AddRange(answers);
    }

    private Question(int index, string title, IEnumerable<Answer> answers)
    {
        Index = index;
        Title = title;

        _answers.AddRange(
            answers.Select((answer, index) 
                => answer.WithIndex(index))
        );
    }

    public Question WithIndex(int index)
        => new(index, Title, Answers);
}