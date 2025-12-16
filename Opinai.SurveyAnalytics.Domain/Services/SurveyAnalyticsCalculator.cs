using Opinai.SurveyAnalytics.Domain.Models.Inputs;
using Opinai.SurveyAnalytics.Domain.Models.Results;

namespace Opinai.SurveyAnalytics.Domain.Services;

public class SurveyAnalyticsCalculator
{
    public SurveyAnalyticsResult Calculate(SurveyAnalyticsInput input)
    {
        var questions = input.Questions.Select(q =>
        {
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
