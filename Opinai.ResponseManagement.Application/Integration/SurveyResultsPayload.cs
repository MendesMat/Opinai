namespace Opinai.ResponseManagement.Application.Integration;

public record SurveyResultsPayload(
    Guid SurveyId,
    IReadOnlyCollection<QuestionResultsPayload> Questions
);
