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

    public void UpdateMetadata(string? title, string? description, Status? status)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        if (status.HasValue)
            Status = status.Value;
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
}
