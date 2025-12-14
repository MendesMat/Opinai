using Microsoft.AspNetCore.Mvc;
using Opinai.Shared.Api.Controllers;
using Opinai.SurveyManagement.Application.Dtos.Survey;
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
        if (!updated)
            return NotFound(new { message = "Recurso não encontrado." });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Recurso não encontrado." });

        return NoContent();
    }
}
