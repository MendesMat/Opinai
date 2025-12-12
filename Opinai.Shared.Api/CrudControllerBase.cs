using Microsoft.AspNetCore.Mvc;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Api;

public class CrudControllerBase<TDto, TCreateDto, TUpdateDto>
    (ICrudService<TDto, TCreateDto, TUpdateDto> service)
    : ControllerBase
{
    private readonly ICrudService<TDto, TCreateDto, TUpdateDto> _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TCreateDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user is null)
            return NotFound(new { message = "Recurso não encontrado." });

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Recurso não encontrado." });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Recurso não encontrado." });

        return NoContent();
    }
}
