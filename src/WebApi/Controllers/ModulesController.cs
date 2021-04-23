using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.WebApi.Extensions;
using DeveloperPath.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.WebApi.Controllers
{
    //[Authorize]
    [Route("api/paths/{pathId}/modules")]
  public class ModulesController : ApiController
  {
    public ModulesController(IMediator mediator)
    {
      _mediator = mediator;
    }

    /// <summary>
    /// Get all available modules
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="requestParams">Request parameters</param>
    /// <returns>A collection of modules with summary information</returns>
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> Get(int pathId, [FromQuery] RequestParams requestParams = null)
    {
      //TODO: consider adding default page size and show 1st page instead of all
      return requestParams is not null && requestParams.UsePaging()
        ? await GetPage(pathId, requestParams)
        : await GetAll(pathId);
    }

    /// <summary>
    /// Get module information by its Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <returns>Information about the module</returns>
    [HttpGet("{moduleId}", Name = "GetModule")]
    [HttpHead("{moduleId}")]
    public async Task<ActionResult<ModuleDto>> Get(int pathId, int moduleId)
    {
      ModuleDto model = await Mediator.Send(new GetModuleQuery { PathId = pathId, Id = moduleId });

      return Ok(model);
    }

    ///// <summary>
    ///// Get module details information by its Id
    ///// </summary>
    ///// <param name="pathId">An id of the path</param>
    ///// <param name="moduleId">An id of the module</param>
    ///// <returns>Detailed information of the module with themes included</returns>
    //[Route("api/paths/{pathId}/moduledetails")]
    //[HttpGet("{moduleId}", Name = "GetModuleDetails")]
    //[HttpHead("{moduleId}")]
    //public async Task<ActionResult<ModuleViewModel>> GetDetails(int pathId, int moduleId)
    //{
    //  ModuleViewModel model = await Mediator.Send(new GetModuleDetailsQuery { Id = moduleId });

    //  return Ok(model);
    //}

    /// <summary>
    /// Create a module
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="command">Module object</param>
    /// <returns>Created module</returns>
    [HttpPost]
    public async Task<ActionResult<ModuleDto>> Create(int pathId,
      [FromBody] CreateModuleCommand command)
    {
      if (pathId != command.PathId)
        return BadRequest();

      ModuleDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetModule", new { pathId = command.PathId, moduleId = model.Id }, model);
    }

    /// <summary>
    /// Update the module with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <param name="command">Updated module object</param>
    /// <returns>Updated module</returns>
    [HttpPut("{moduleId}")]
    public async Task<ActionResult<ModuleDto>> Update(int pathId, int moduleId,
      [FromBody] UpdateModuleCommand command)
    {
      if (moduleId != command.Id)
        return BadRequest();

      return Ok(await Mediator.Send(command));
    }

    // TODO: add PATCH

    /// <summary>
    /// Delete the module with given Id from path with given Id
    /// </summary>
    /// <param name="pathId">An id of the path</param>
    /// <param name="moduleId">An id of the module</param>
    /// <returns></returns>
    [HttpDelete("{moduleId}")]
    public async Task<ActionResult> Delete(int pathId, int moduleId)
    {
      await Mediator.Send(new DeleteModuleCommand { PathId = pathId, Id = moduleId });

      return NoContent();
    }

    private async Task<ActionResult<IEnumerable<ModuleDto>>> GetAll(int pathId)
    {
      IEnumerable<ModuleDto> model = await Mediator.Send(new GetModuleListQuery { PathId = pathId });
      return Ok(model);
    }

    private async Task<ActionResult<IEnumerable<ModuleDto>>> GetPage(int pathId, RequestParams filter)
    {
      var (paginationData, result) = await Mediator.Send(
          new GetModuleListQueryPaging()
          {
            PathId = pathId,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
          });

      Response?.Headers?.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationData));
      return Ok(result);
    }
  }
}