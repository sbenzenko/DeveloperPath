using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Modules.Commands.DeleteModule;
using DeveloperPath.Application.Modules.Commands.UpdateModule;
using DeveloperPath.Application.Modules.Queries.GetModules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Controllers
{
  //[Authorize]
  public class ModulesController : ApiController
  {
    /// <summary>
    /// Get module information by its Id
    /// </summary>
    /// <param name="moduleId">An id of the module</param>
    /// <returns>Detailed information of the module with themes included</returns>
    [HttpGet("{moduleId}", Name = "GetModule")]
    [HttpHead("{moduleId}")]
    public async Task<ActionResult<ModuleViewModel>> Get(int moduleId)
    {
      ModuleViewModel model = await Mediator.Send(new GetModuleQuery { Id = moduleId });

      return Ok(model);
    }

    /// <summary>
    /// Create a module
    /// </summary>
    /// <param name="command">Module object</param>
    /// <returns>Created module</returns>
    [HttpPost]
    public async Task<ActionResult<ModuleDto>> Create(CreateModuleCommand command)
    {
      ModuleDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetModule", new { moduleId = model.Id }, model);
    }

    /// <summary>
    /// Update the module with given Id
    /// </summary>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="command">Updated module object</param>
    /// <returns>Updated module</returns>
    [HttpPut("{moduleId}")]
    public async Task<ActionResult<ModuleDto>> Update(int moduleId, UpdateModuleCommand command)
    {
      if (moduleId != command.Id)
      {
        return BadRequest();
      }

      return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Delete the module with given Id
    /// </summary>
    /// <param name="moduleId">An id of the module</param>
    /// <returns></returns>
    [HttpDelete("{moduleId}")]
    public async Task<ActionResult> Delete(int moduleId)
    {
      await Mediator.Send(new DeleteModuleCommand { Id = moduleId });

      return NoContent();
    }
  }
}
