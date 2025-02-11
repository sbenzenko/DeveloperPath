using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.DeletePath;
using DeveloperPath.Application.CQRS.Paths.Commands.PatchPath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Filters;
using DeveloperPath.WebApi.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers;

[Route("api/paths")]
[Authorize]
public class PathsController : ApiController
{
  public PathsController(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Get all available paths
  /// </summary>
  /// <param name="requestParams">Page size, page number</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>A collection of paths with summary information</returns>
  /// <response code="200">Returns a list of paths</response>
  [HttpGet(Name = "GetPaths")]
  [HttpHead]
  [AllowAnonymous]
  [ServiceFilter(typeof(PagedListResultFilterAttribute))]
  public async Task<ActionResult<IEnumerable<Path>>> Get([FromQuery] PathRequestParams requestParams = null, CancellationToken ct = default)
  {
    var result = await _mediator.Send(new GetPathPagedListQuery()
    {
      PageSize = requestParams.PageSize,
      PageNumber = requestParams.PageNumber,
      OnlyVisible = requestParams.OnlyVisible
    }, ct);

    return Ok(result);
  }



  /// <summary>
  /// Get all deleted paths
  /// </summary>
  /// <param name="requestParams">Page size, page number</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>A collection of paths with summary information</returns>
  /// <response code="200">Returns a list of paths</response>
  [HttpGet("deleted")]
  [HttpHead("deleted")]
  [Authorize(Roles = "Administrator")]
  public async Task<ActionResult<IEnumerable<DeletedPath>>> GetDeleted([FromQuery] RequestParams requestParams = null, CancellationToken ct = default)
  {
    var result = await _mediator.Send(new GetDeletedPathListQuery()
    {
      PageSize = requestParams.PageSize,
      PageNumber = requestParams.PageNumber,
    }, ct);

    return Ok(result);
  }

  ///// <summary>
  ///// Get path information by its Id
  ///// </summary>
  ///// <param name="pathId">An id of the path</param>
  ///// <param name="ct"></param>
  ///// <returns>Information of the path with modules included</returns>
  ///// <response code="200">Returns requested path</response>
  //[HttpGet("{pathId}", Name = "GetPath")]
  //[HttpHead("{pathId}")]
  //public async Task<ActionResult<Path>> Get(int pathId, CancellationToken ct = default)
  //{
  //  Path model = await Mediator.Send(new GetPathQuery { Id = pathId }, ct);

  //  return Ok(model);
  //}

  /// <summary>
  /// Get detailed path information by its Id
  /// </summary>
  /// <param name="pathId">An id of the path</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>Detailed information of the path with modules included</returns>
  /// <response code="200">Returns requested path</response>
  [HttpGet("{pathId}", Name = "GetPath")]
  [HttpHead("{pathId}")]
  [AllowAnonymous]
  public async Task<ActionResult<PathDetails>> Get(int pathId, CancellationToken ct = default)
  {
    var model = await _mediator.Send(new GetPathDetailsQuery { Id = pathId }, ct);

    return Ok(model);
  }

  /// <summary>
  /// Create a path
  /// </summary>
  /// <param name="command">Path object</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>Created path</returns>
  /// <response code="201">Path created successfully</response>
  /// <response code="406">Not acceptable entity provided</response>
  /// <response code="415">Unsupported media type provided</response>
  /// <response code="422">Unprocessible entity provided</response>
  [Authorize(Roles = "Administrator")]
  [HttpPost]
  //TODO: adding "application/xml" causes "An error occurred while deserializing input data." error for some reason
  [Consumes("application/json")]
  public async Task<ActionResult<Path>> Create([FromBody] CreatePath command, CancellationToken ct = default)
  {
    Path model = await _mediator.Send(command, ct);

    return CreatedAtRoute("GetPath", new { pathId = model.Id }, model);
  }

  /// <summary>
  /// Update the path with given Id
  /// </summary>
  /// <param name="pathId">An id of the path</param>
  /// <param name="command">Updated path</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>Updated path</returns>
  /// <response code="200">Path updated successfully</response>
  /// <response code="406">Not acceptable entity provided</response>
  /// <response code="415">Unsupported media type provided</response>
  /// <response code="422">Unprocessible entity provided</response>
  [Authorize(Roles = "Administrator")]
  [HttpPut("{pathId}")]
  [Consumes("application/json")]
  public async Task<ActionResult<Path>> Update(int pathId,
    [FromBody] UpdatePath command, CancellationToken ct = default)
  {
    if (pathId != command.Id)
      return BadRequest();

    return Ok(await _mediator.Send(command, ct));
  }

  /// <summary>
  /// Update the part of path with given Id
  /// </summary>
  /// <param name="patchDocument"></param>
  /// <param name="pathId">An id of the path</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>Updated path</returns>
  /// <response code="200">Path updated successfully</response>
  /// <response code="406">Not acceptable entity provided</response>
  /// <response code="415">Unsupported media type provided</response>
  /// <response code="422">Unprocessible entity provided</response>

  [HttpPatch("{pathId}")]
  [Authorize(Roles = "Administrator")]
  public async Task<ActionResult<Path>> Patch([FromBody] JsonPatchDocument patchDocument, [FromRoute] int pathId, CancellationToken ct = default)
  {
    var pathPatchCommand = new PatchPathCommand(pathId, patchDocument);
    return Ok(await _mediator.Send(pathPatchCommand, ct));
  }

  /// <summary>
  /// Update the part of deleted path with given Id. Common case is restoring deleted Path
  /// </summary>
  /// <param name="patchDocument"></param>
  /// <param name="pathId">An id of the path</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns>Updated path</returns>
  /// <response code="200">Path updated successfully</response>
  /// <response code="406">Not acceptable entity provided</response>
  /// <response code="415">Unsupported media type provided</response>
  /// <response code="422">Unprocessible entity provided</response>

  [HttpPatch("deleted/{pathId}")]
  [Authorize(Roles = "Administrator")]
  [Consumes("application/json")]
  public async Task<ActionResult<Path>> PatchDeleted([FromBody] JsonPatchDocument patchDocument, [FromRoute] int pathId, CancellationToken ct = default)
  {
    var pathPatchCommand = new PatchPathCommand(pathId, patchDocument, false);
    return Ok(await _mediator.Send(pathPatchCommand, ct));
  }

  /// <summary>
  /// Delete the path with given Id
  /// </summary>
  /// <param name="pathId">An id of the path</param>
  /// <param name="ct">Cancellation token</param>
  /// <returns></returns>
  /// <response code="204">Path deleted successfully</response>
  [Authorize(Roles = "Administrator")]
  [HttpDelete("{pathId}")]
  public async Task<IActionResult> Delete(int pathId, CancellationToken ct = default)
  {
    await _mediator.Send(new DeletePath { Id = pathId }, ct);

    return NoContent();
  }
}