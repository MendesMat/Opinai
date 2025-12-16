using Microsoft.AspNetCore.Mvc;
using Opinai.SurveyAnalytics.Application.Contracts;
using Opinai.SurveyAnalytics.Application.Interfaces;

namespace Opinai.SurveyAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyAnalyticsController(ISurveyAnalyticsService service) 
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Build(Guid surveyId, [FromBody] SurveyResultsPayload payload)
    {
        if (surveyId != payload.SurveyId) return BadRequest("Id da pesquisa incompatível.");

        var result = await service.BuildAsync(payload);
        return Ok(result);
    }
}
