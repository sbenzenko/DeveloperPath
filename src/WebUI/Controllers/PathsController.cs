using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.DeletePath;
using DeveloperPath.Application.Paths.Commands.UpdatePath;
using DeveloperPath.Application.Paths.Queries.GetPaths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Controllers
{
  //[Authorize]
  public class PathsController : ApiController
  {
    /// <summary>
    /// Get all available paths
    /// </summary>
    /// <returns>A collection of paths with summary information</returns>
    [HttpGet]
    public async Task<IEnumerable<PathDto>> Get()
    {
      return await Mediator.Send(new GetPathListQuery());
    }

    /// <summary>
    /// Get path information by its Id
    /// </summary>
    /// <param name="id">An id of the path</param>
    /// <returns>Detailed information of the path with modules included</returns>
    [HttpGet("{id}")]
    public async Task<PathDetailsDto> Get(int id)
    {
      return await Mediator.Send(new GetPathQuery { Id = id });
    }

    /// <summary>
    /// Create a path
    /// </summary>
    /// <param name="command">Path object</param>
    /// <returns>An Id of created path</returns>
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreatePathCommand command)
    {
      return await Mediator.Send(command);
    }

    /// <summary>
    /// Update the path with given Id
    /// </summary>
    /// <param name="id">An id of the path</param>
    /// <param name="command">Updated path object</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdatePathCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest();
      }

      await Mediator.Send(command);

      return NoContent();
    }

    /// <summary>
    /// Delete the path with given Id
    /// </summary>
    /// <param name="id">An id of the path</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
      await Mediator.Send(new DeletePathCommand { Id = id });

      return NoContent();
    }
  }
}
