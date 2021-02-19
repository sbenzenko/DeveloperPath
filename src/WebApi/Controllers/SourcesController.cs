using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Sources.Commands.CreateSource;
using DeveloperPath.Application.Sources.Commands.DeleteSource;
using DeveloperPath.Application.Sources.Commands.UpdateSource;
using DeveloperPath.Application.Sources.Queries.GetSources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperPath.WebApi.Controllers
{
  //[Authorize]
  [Route("api/paths/{pathId}/modules/{moduleId}/themes/{themeId}/sources")]
  public class SourcesController : ApiController
  {
    /// <summary>
    /// Get all available sources
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the module</param>
    /// <returns>A collection of sources with summary information</returns>
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<SourceDto>>> Get(int pathId, int moduleId, int themeId)
    {
      IEnumerable<SourceDto> sources = await Mediator.Send(
         new GetSourceListQuery { PathId = pathId, ModuleId = moduleId, ThemeId = themeId });

      return Ok(sources);
    }

    /// <summary>
    /// Get source information by its Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the theme</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="sourceId">An id of the source</param>
    /// <returns>Detailed information of the source with sources included</returns>
    [HttpGet("{sourceId}", Name = "GetSource")]
    [HttpHead("{sourceId}")]
    public async Task<ActionResult<SourceDto>> Get(int pathId, int moduleId, int themeId, int sourceId)
    {
      SourceDto model = await Mediator.Send(
        new GetSourceQuery { PathId = pathId, ModuleId = moduleId, ThemeId = themeId, Id = sourceId });

      return Ok(model);
    }

    ///// <summary>
    ///// Get detailed source information by its Id
    ///// </summary>
    ///// <param name="pathId">An id of the path</param>
    ///// <param name="moduleId">An id of the theme</param>
    ///// <param name="themeId">An id of the theme</param>
    ///// <param name="sourceId">An id of the source</param>
    ///// <returns>Detailed information of the source with sources included</returns>
    //// TODO: Feature enhancement: this item will also have ratings and user comments
    //[Route("api/paths/{pathId}/modules/{moduleId}/themes/{themeId}/sourcedetails")]
    //[HttpGet("{sourceId}", Name = "GetSourceDetails")]
    //[HttpHead("{sourceId}")]
    //public async Task<ActionResult<SourceViewModel>> GetDetails(int pathId, int moduleId, int themeId, int sourceId)
    //{
    //  SourceViewModel model = await Mediator.Send(
    //    new GetSourceDetailsQuery { PathId = pathId, ModuleId = moduleId, ThemeId = themeId, Id = sourceId });

    //  return Ok(model);
    //}

    /// <summary>
    /// Create a source
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="command">Command object</param>
    /// <returns>Created source</returns>
    [HttpPost]
    public async Task<ActionResult<SourceDto>> Create(int pathId, int moduleId, int themeId,
      [FromBody] CreateSourceCommand command)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId || themeId != command.ThemeId)
        return BadRequest();

      SourceDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetSource", 
        new { pathId = command.PathId, moduleId = command.ModuleId, themeId = model.ThemeId, sourceId = model.Id }, model);
    }

    /// <summary>
    /// Update the source with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="sourceId">An id of the source</param>
    /// <param name="command">Updated source object</param>
    /// <returns>Updated spurce</returns>
    [HttpPut("{sourceId}")]
    public async Task<ActionResult<SourceDto>> Update(int pathId, int moduleId, int themeId, int sourceId, 
      [FromBody] UpdateSourceCommand command)
    {
      if (pathId != command.PathId || moduleId != command.ModuleId || 
          themeId != command.ThemeId || sourceId != command.Id)
        return BadRequest();

      return Ok(await Mediator.Send(command));
    }

    // TODO: add PATCH

    /// <summary>
    /// Delete the source with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">And id of the module</param>
    /// <param name="themeId">And id of the theme</param>
    /// <param name="sourceId">An id of the source</param>
    /// <returns></returns>
    [HttpDelete("{sourceId}")]
    public async Task<ActionResult> Delete(int pathId, int moduleId, int themeId, int sourceId)
    {
      await Mediator.Send(new DeleteSourceCommand { PathId = pathId, ModuleId = moduleId, ThemeId = themeId, Id = sourceId });

      return NoContent();
    }
  }
}
