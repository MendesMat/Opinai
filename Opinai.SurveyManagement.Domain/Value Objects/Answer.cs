namespace Opinai.SurveyManagement.Domain;

public class Answer
{
    public int Index { get; }
    public string Text { get; }

    public Answer(string text)
    {
        Index = -1;
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