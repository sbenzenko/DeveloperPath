﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
using DeveloperPath.Shared.ClientModels;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
  [Route("api/paths/{pathId}/modules/{moduleId}/themes")]
  public class ThemesController : ApiController
  {
    public ThemesController(IMediator mediator)
    {
      _mediator = mediator;
    }

    /// <summary>
    /// Get all available themes
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A collection of themes with summary information</returns>
    /// <response code="200">Returns a list of themes in the module</response>
    /// <response code="404">Module not found</response>
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<Theme>>> Get(int pathId, int moduleId, CancellationToken ct = default)
    {
      IEnumerable<Theme> themes = await _mediator.Send(
         new GetThemeListQuery { PathId = pathId, ModuleId = moduleId }, ct);

      return Ok(themes);
    }

    /// <summary>
    /// Get theme information by its Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Information about the theme</returns>
    /// <response code="200">Returns requested theme</response>
    [HttpGet("{themeId}", Name = "GetTheme")]
    [HttpHead("{themeId}")]
    public async Task<ActionResult<Theme>> Get(int pathId, int moduleId, int themeId, CancellationToken ct = default)
    {
      Theme model = await _mediator.Send(
        new GetThemeQuery { PathId = pathId, ModuleId = moduleId, Id = themeId }, ct);

      return Ok(model);
    }

    ///// <summary>
    ///// Get detailed theme information by its Id
    ///// </summary>
    ///// <param name="pathId">An id of the path</param>
    ///// <param name="moduleId">An id of the theme</param>
    ///// <param name="themeId">An id of the theme</param>
    ///// <returns>Detailed information about the theme with sources included</returns>
    //[Route("api/paths/{pathId}/modules/{moduleId}/themedetails")]
    //[HttpGet("{themeId}", Name = "GetThemeDetails")]
    //[HttpHead("{themeId}")]
    //public async Task<ActionResult<ThemeViewModel>> GetDetails(int pathId, int moduleId, int themeId)
    //{
    //  ThemeViewModel model = await Mediator.Send(
    //    new GetThemeDetailsQuery { PathId = pathId, ModuleId = moduleId, Id = themeId });

    //  return Ok(model);
    //}

    /// <summary>
    /// Create a theme
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="command">Theme object</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created theme</returns>
    /// <response code="201">Theme created successfully</response>
    /// <response code="404">Module not found</response>
    /// <response code="406">Not acceptable entity provided</response>
    /// <response code="415">Unsupported media type provided</response>
    /// <response code="422">Unprocessible entity provided</response>
    [Authorize]
    [HttpPost]
    [Consumes("application/json")]
    public async Task<ActionResult<Theme>> Create(int pathId, int moduleId,
      [FromBody] CreateTheme command, CancellationToken ct = default)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId)
        return BadRequest();

      Theme model = await _mediator.Send(command, ct);

      return CreatedAtRoute("GetTheme",
        new { pathId = command.PathId, moduleId = model.ModuleId, themeId = model.Id }, model);
    }

    /// <summary>
    /// Update the theme with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="command">Updated theme object</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated theme</returns>
    /// <response code="200">Theme updated successfully</response>
    /// <response code="406">Not acceptable entity provided</response>
    /// <response code="415">Unsupported media type provided</response>
    /// <response code="422">Unprocessible entity provided</response>
    [Authorize]
    [HttpPut("{themeId}")]
    [Consumes("application/json")]
    public async Task<ActionResult<Theme>> Update(int pathId, int moduleId, int themeId,
      [FromBody] UpdateTheme command, CancellationToken ct = default)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId || themeId != command.Id)
        return BadRequest();

      return Ok(await _mediator.Send(command, ct));
    }

    // TODO: add PATCH

    /// <summary>
    /// Delete the theme with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">And id of the module the theme is in</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <response code="204">Theme deleted successfully</response>
    [Authorize]
    [HttpDelete("{themeId}")]
    public async Task<ActionResult> Delete(int pathId, int moduleId, int themeId, CancellationToken ct = default)
    {
      await _mediator.Send(new DeleteTheme { PathId = pathId, ModuleId = moduleId, Id = themeId }, ct);

      return NoContent();
    }
  }
}
