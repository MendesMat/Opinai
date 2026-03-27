using Opinai.SurveyAnalytics.Domain.Models.Inputs;
using Opinai.SurveyAnalytics.Domain.Models.Results;

namespace Opinai.SurveyAnalytics.Domain.Services;

public class SurveyAnalyticsCalculator
{
    public static SurveyAnalyticsResult Calculate(SurveyAnalyticsInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.SurveyId == Guid.Empty)
            throw new ArgumentException("SurveyId não pode ser vazio.");

        ArgumentNullException.ThrowIfNull(input.Questions);
        
        if (!input.Questions.Any())
            throw new ArgumentException("A lista de questões não pode ser vazia.");

        var questions = input.Questions.Select(q =>
        {
            ArgumentNullException.ThrowIfNull(q.Answers);

            var total = q.Answers.Sum(a => a.Count);

            var answers = q.Answers.Select(a =>
                new AnswerAnalyticsResult(
                    a.AnswerIndex,
                    a.Count,
                    total == 0 ?
                        0 : Math.Round((decimal)a.Count / total * 100, 2)
                )
            ).ToList();

            return new QuestionAnalyticsResult(
                q.QuestionIndex,
                total,
                answers
            );
        }).ToList();

        return new SurveyAnalyticsResult(
            input.SurveyId,
            questions
        );
    }
}
