using Microsoft.AspNetCore.Mvc;
using Opinai.ResponseManagement.Application.Dtos;
using Opinai.ResponseManagement.Application.Interfaces;

namespace Opinai.ResponseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyResponseController(
    ISurveyResponseService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateResponse([FromBody] SurveyResponseDto dto)
    {
        await service.AddSurveyResponseAsync(dto);
        return NoContent();
    }
}
