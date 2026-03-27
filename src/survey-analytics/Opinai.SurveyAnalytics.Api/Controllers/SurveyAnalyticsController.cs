using Microsoft.AspNetCore.Mvc;
using Opinai.Messaging.Contracts.Payloads;
using Opinai.SurveyAnalytics.Application.Interfaces;

namespace Opinai.SurveyAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyAnalyticsController(ISurveyAnalyticsService service) 
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Build([FromBody] SurveyResultsPayload payload)
    {
        var result = await service.BuildAsync(payload);
        return Ok(result);
    }
}
