using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Sources.Commands.CreateSource;
using DeveloperPath.Application.Sources.Commands.DeleteSource;
using DeveloperPath.Application.Sources.Commands.UpdateSource;
using DeveloperPath.Application.Sources.Queries.GetSource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Controllers
{
  //[Authorize]
  [Route("api/modules/{moduleId}/themes/{themeId}/sources")]
  public class SourcesController : ApiController
  {
    /// <summary>
    /// Get source information by its Id
    /// </summary>
    /// <param name="moduleId">An id of the theme</param>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="sourceId">An id of the source</param>
    /// <returns>Detailed information of the source with sources included</returns>
    // TODO: Feature enhancement: this item will also have ratings and user comments
    [HttpGet("{sourceId}", Name = "GetSource")]
    [HttpHead("{sourceId}")]
    public async Task<ActionResult<SourceViewModel>> Get(int moduleId, int themeId, int sourceId)
    {
      SourceViewModel model = await Mediator.Send(
        new GetSourceQuery { ModuleId = moduleId, ThemeId = themeId, Id = sourceId });

      return Ok(model);
    }

    /// <summary>
    /// Create a source
    /// </summary>
    /// <param name="command">Command object</param>
    /// <returns>Created source</returns>
    [HttpPost]
    public async Task<ActionResult<SourceDto>> Create(CreateSourceCommand command)
    {
      SourceDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetSource", 
        new { moduleId = command.ModuleId, themeId = model.ThemeId, sourceId = model.Id }, model);
    }

    /// <summary>
    /// Update the source with given Id
    /// </summary>
    /// <param name="sourceId">An id of the source</param>
    /// <param name="command">Updated source object</param>
    /// <returns>Updated spurce</returns>
    [HttpPut("{sourceId}")]
    public async Task<ActionResult<SourceDto>> Update(int sourceId, UpdateSourceCommand command)
    {
      if (sourceId != command.Id)
      {
        return BadRequest();
      }

      return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Delete the source with given Id
    /// </summary>
    /// <param name="moduleId">And id of the module</param>
    /// <param name="themeId">And id of the theme</param>
    /// <param name="sourceId">An id of the source</param>
    /// <returns></returns>
    [HttpDelete("{sourceId}")]
    public async Task<ActionResult> Delete(int moduleId, int themeId, int sourceId)
    {
      await Mediator.Send(new DeleteSourceCommand { ModuleId = moduleId, ThemeId = themeId, Id = sourceId });

      return NoContent();
    }
  }
}
