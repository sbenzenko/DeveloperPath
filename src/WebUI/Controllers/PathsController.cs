using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.DeletePath;
using DeveloperPath.Application.Paths.Commands.UpdatePath;
using DeveloperPath.Application.Paths.Queries.GetPaths;
using DeveloperPath.Application.Common.Models;
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
    [HttpHead]
    public async Task<ActionResult<IEnumerable<PathDto>>> Get()
    {
      IEnumerable<PathDto> model = await Mediator.Send(new GetPathListQuery());

      return Ok(model);
    }

    /// <summary>
    /// Get path information by its Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <returns>Detailed information of the path with modules included</returns>
    [HttpGet("{pathId}", Name = "GetPath")]
    [HttpHead("{pathId}")]
    public async Task<ActionResult<PathDto>> Get(int pathId)
    {
      PathViewModel model = await Mediator.Send(new GetPathQuery { Id = pathId });

      return Ok(model);
    }

    /// <summary>
    /// Create a path
    /// </summary>
    /// <param name="command">Path object</param>
    /// <returns>A created path</returns>
    [HttpPost]
    public async Task<ActionResult<PathDto>> Create(CreatePathCommand command)
    {
      PathDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetPath", new { pathId = model.Id }, model);
    }

    /// <summary>
    /// Update the path with given Id
    /// </summary>
    /// <param name="id">An id of the path</param>
    /// <param name="command">Updated path object</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PathDto>> Update(int id, UpdatePathCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest();
      }

      return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Delete the path with given Id
    /// </summary>
    /// <param name="id">An id of the path</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      await Mediator.Send(new DeletePathCommand { Id = id });

      return NoContent();
    }
  }
}
