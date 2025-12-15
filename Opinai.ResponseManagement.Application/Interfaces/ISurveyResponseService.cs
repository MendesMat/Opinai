using Opinai.ResponseManagement.Application.Dtos;

namespace Opinai.ResponseManagement.Application.Interfaces;

public interface ISurveyResponseService
{
    Task AddSurveyResponseAsync(SurveyResponseDto dto);
}