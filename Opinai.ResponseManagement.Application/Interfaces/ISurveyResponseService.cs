using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Application.Enums;
using Opinai.ResponseManagement.Application.Integration;

namespace Opinai.ResponseManagement.Application.Interfaces;

public interface ISurveyResponseService
{
    Task<SurveyResponseResult> AddSurveyResponseAsync(SurveyResponseDto dto)
    Task<SurveyResultsPayload> BuildSurveyResultsAsync(Guid surveyId);
}