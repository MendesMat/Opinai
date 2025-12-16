using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Domain.Enums;

namespace Opinai.SurveyManagement.Domain.Entities;

public class Survey : IEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; } = Status.Draft;

    private readonly List<Question> _questions = [];
    public IReadOnlyCollection<Question> Questions => _questions;

    public Survey(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public bool CanEdit() => Status == Status.Draft;
    public bool CanPublish() => Status == Status.Draft;
    public bool CanFinish() => Status == Status.Published;

    public void UpdateMetadata(string? title, string? description)
    {
        if(!CanEdit())
            throw new InvalidOperationException
                ("Apenas pesquisas em rascunho podem ser alteradas.");

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;
    }

    public void ReplaceQuestions(IEnumerable<Question> questions)
    {
        _questions.Clear();
        _questions.AddRange(Reindex(questions));
    }

    private static IEnumerable<Question> Reindex(IEnumerable<Question> questions)
    {
        var questionIndex = 0;

        foreach (var question in questions)
            yield return question.WithIndex(questionIndex++);
    }

    public void PublishSurvey()
    {
        if(!CanPublish())
            throw new InvalidOperationException(
                "Apenas pesquisas em rascunho podem ser publicadas.");

        Status = Status.Published;
    }

    public void FinishSurvey()
    {
        if(!CanFinish())
            throw new InvalidOperationException
                ("Apenas pesquisas publicadas podem ser concluídas.");

        Status = Status.Finished;
    }
}
