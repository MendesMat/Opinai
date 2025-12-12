using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Domain.Enums;

namespace Opinai.SurveyManagement.Domain.Entities;

public class Survey( 
    string title, 
    string description) : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public Status Status { get; set; } = Status.Draft;

    public List<Question> Questions { get; set; } = [];
}
