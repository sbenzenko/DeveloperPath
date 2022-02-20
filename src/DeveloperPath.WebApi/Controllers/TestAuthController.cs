using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TestAuthController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("access allowed");
    }
}