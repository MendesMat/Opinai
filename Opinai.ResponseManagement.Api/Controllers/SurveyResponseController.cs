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
    public IActionResult CreateResponse([FromBody] SurveyResponseDto dto)
    {
        service.AddSurveyResponseAsync(dto);
        return NoContent();
    }
}
