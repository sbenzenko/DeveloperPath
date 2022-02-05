using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
 

namespace DeveloperPath.WebApi.Controllers
{
    [Route("api/modules")]
    public class ModulesController : ApiController
    {
        public ModulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available modules
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>A collection of modules</returns>
        /// <response code="200">Returns a list of modules</response>
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ModuleTitle>>> Get(CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetAllAvailableModulesQuery(), ct);
            return Ok(result);
        }

        /// <summary>
        /// Get list of modules for given path key
        /// </summary>
        /// <param name="pathId">Path ID</param>
        /// <param name="ct"></param>
        /// <returns>A collection of modules</returns>
        /// <response code="200">Returns a list of modules</response>
        /// <response code="404">No modules found</response>
        [HttpGet("{pathId}", Name = "GetPathModules")]
        [HttpHead("{pathId}")]
        public async Task<ActionResult<IEnumerable<Module>>> Get(int pathId, CancellationToken ct = default)
        {
            IEnumerable<Module> model = await Mediator.Send(new GetModuleListQuery { PathId = pathId }, ct);
            return Ok(model);
        }

        /// <summary>
        /// Get module information by its Id
        /// </summary>
        /// <param name="pathKey">Path key</param>
        /// <param name="moduleId">An id of the module</param>
        /// <param name="ct"></param>
        /// <returns>Information about the module</returns>
        /// <response code="200">Returns requested module</response>
        /// <response code="404">Module not found</response>
        [HttpGet("{pathKey}/{moduleId}", Name = "GetModuleById")]
        [HttpHead("{pathKey}/{moduleId}")]
        public async Task<ActionResult<Module>> Get(string pathKey, int moduleId, CancellationToken ct = default)
        {
            Module model = await Mediator.Send(new GetModuleQuery { Id = moduleId, PathKey = pathKey }, ct);

            return Ok(model);
        }

        /// <summary>
        /// Create a module
        /// </summary>
        /// <param name="command">Module object</param>
        /// <returns>Created module</returns>
        /// <response code="201">Module created successfully</response>
        /// <response code="404">Path not found</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        [Authorize]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Module>> Create([FromBody] CreateModule command)
        {
            Module model = await Mediator.Send(command);

            return CreatedAtRoute("GetModule", new { moduleId = model.Id }, model);
        }

        /// <summary>
        /// Update the module with given Id
        /// </summary>
        /// <param name="moduleId">An id of the module</param>
        /// <param name="command">Updated module object</param>
        /// <returns>Updated module</returns>
        /// <response code="200">Module updated successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        [Authorize]
        [HttpPut("{moduleId}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Module>> Update(int moduleId,
          [FromBody] UpdateModule command)
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
        /// <response code="204">Module deleted successfully</response>
        [Authorize]
        [HttpDelete("{moduleId}")]
        public async Task<ActionResult> Delete(int pathId, int moduleId)
        {
            await Mediator.Send(new DeleteModule { PathId = pathId, Id = moduleId });

            return NoContent();
        }

        private async Task<ActionResult<IEnumerable<Module>>> GetPage(string pathKey, RequestParams filter, CancellationToken ct = default)
        {
            var (paginationData, result) = await Mediator.Send(
                new GetModuleListQueryPaging()
                {
                    PathKey = pathKey,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                }, ct);

            Response?.Headers?.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationData));
            return Ok(result);
        }
    }
}