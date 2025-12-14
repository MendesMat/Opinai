using Microsoft.AspNetCore.Mvc;

namespace Opinai.ResponseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponseController
{
    [HttpPost]
    public IActionResult CreateResponse([FromBody] string input)
    {
        
        return new OkObjectResult($"Response created for input: {input}");
    }
}
