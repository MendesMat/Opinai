using Microsoft.AspNetCore.Mvc;
using Opinai.Shared.Api.Controllers;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Enums;
using Opinai.SurveyManagement.Application.Interface;

namespace Opinai.SurveyManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController(ISurveyService service)
    : QueryControllerBase<SurveyDto>(service)
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSurveyDto dto)
    {
        var id = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSurveyDto dto)
    {
        var updated = await service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> PublishSurvey(Guid id)
    {
        return ToActionResult(
                await service.PublishSurveyAsync(id),
                "Apenas pesquisas em rascunho podem ser publicadas."
        );
    }

    [HttpPost("{id}/finish")]
    public async Task<IActionResult> FinishSurvey(Guid id)
    {
        return ToActionResult(
                await service.FinishSurveyAsync(id),
                "Apenas pesquisas publicadas podem ser concluídas."
        );
    }

    private IActionResult ToActionResult(
        SurveyActionResult result,
        string invalidStateMessage)
    {
        return result switch
        {
            SurveyActionResult.Success => NoContent(),
            SurveyActionResult.NotFound => NotFound(),
            SurveyActionResult.InvalidState => BadRequest(invalidStateMessage),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
