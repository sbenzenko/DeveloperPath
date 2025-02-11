using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes("application/json")]
public abstract class ApiController : ControllerBase
{
  protected IMediator _mediator;
}