using Microsoft.AspNetCore.Mvc;

namespace AssetLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello World!", status = 200 });
    }
}