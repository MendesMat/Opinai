using Opinai.SurveyAnalytics.Application.Contracts;
using Opinai.SurveyAnalytics.Domain.Models.Results;

namespace Opinai.SurveyAnalytics.Application.Interfaces;

public interface ISurveyAnalyticsService
{
    Task<SurveyAnalyticsResult> BuildAsync(SurveyResultsPayload payload);
}