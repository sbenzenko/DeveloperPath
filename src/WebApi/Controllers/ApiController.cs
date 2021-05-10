using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public abstract class ApiController : ControllerBase
  {
    protected IMediator _mediator;

    // Had to change it to constructor injection for unit tests
    protected IMediator Mediator => _mediator; // ??= HttpContext.RequestServices.GetService<IMediator>();
  }
}
