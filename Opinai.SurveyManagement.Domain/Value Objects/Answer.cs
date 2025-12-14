namespace Opinai.SurveyManagement.Domain;

public class Answer
{
    public int Index { get; private set; } = -1;
    public string Text { get; } = string.Empty;

    public Answer() { }

    public Answer(string text)
    {
        Text = text;
    }

    private Answer(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public Answer WithIndex(int index)
        => new(index, Text);
}