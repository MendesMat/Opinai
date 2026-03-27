using Microsoft.AspNetCore.Mvc;
using Opinai.Shared.Application.Interfaces;

namespace Opinai.Shared.Api.Controllers;

public class QueryControllerBase<TDto>
    (IQueryService<TDto> service)
    : ControllerBase
{
    private readonly IQueryService<TDto> _service = service;

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
}
