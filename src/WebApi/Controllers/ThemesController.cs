using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
 
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
    //[Authorize]
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
    /// <returns>A collection of themes with summary information</returns>
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<ThemeDto>>> Get(int pathId, int moduleId)
    {
      IEnumerable<ThemeDto> themes = await Mediator.Send(
         new GetThemeListQuery { PathId = pathId, ModuleId = moduleId });

      return Ok(themes);
    }

    /// <summary>
    /// Get theme information by its Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the theme</param>
    /// <returns>Information about the theme</returns>
    [HttpGet("{themeId}", Name = "GetTheme")]
    [HttpHead("{themeId}")]
    public async Task<ActionResult<ThemeDto>> Get(int pathId, int moduleId, int themeId)
    {
      ThemeDto model = await Mediator.Send(
        new GetThemeQuery { PathId = pathId, ModuleId = moduleId, Id = themeId });

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
    /// <returns>Created theme</returns>
    [HttpPost]
    public async Task<ActionResult<ThemeDto>> Create(int pathId, int moduleId,
      [FromBody] CreateThemeCommand command)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId)
        return BadRequest();

      ThemeDto model = await Mediator.Send(command);

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
    /// <returns>Upated theme</returns>
    [HttpPut("{themeId}")]
    public async Task<ActionResult<ThemeDto>> Update(int pathId, int moduleId, int themeId,
      [FromBody] UpdateThemeCommand command)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId || themeId != command.Id)
        return BadRequest();

      return Ok(await Mediator.Send(command));
    }

    // TODO: add PATCH

    /// <summary>
    /// Delete the theme with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">And id of the module the theme is in</param>
    /// <param name="themeId">An id of the theme</param>
    /// <returns></returns>
    [HttpDelete("{themeId}")]
    public async Task<ActionResult> Delete(int pathId, int moduleId, int themeId)
    {
      await Mediator.Send(new DeleteThemeCommand { PathId = pathId, ModuleId = moduleId, Id = themeId });

      return NoContent();
    }
  }
}
