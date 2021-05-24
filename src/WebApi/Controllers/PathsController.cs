using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.DeletePath;
using DeveloperPath.Application.CQRS.Paths.Commands.PatchPath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.WebApi.Extensions;
using DeveloperPath.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.ClientModels;


namespace DeveloperPath.WebApi.Controllers
{
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
        /// <returns>A collection of paths with summary information</returns>
        /// <response code="200">Returns a list of paths</response>
        [HttpGet]
        [HttpHead]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Path>>> Get([FromQuery] RequestParams requestParams = null)
        {
            //TODO: consider adding default page size and show 1st page instead of all
            return requestParams is not null && requestParams.UsePaging()
              ? await GetPage(requestParams)
              : await GetAll();
        }


        /// <summary>
        /// Get all deleted paths
        /// </summary>
        /// <returns>A collection of paths with summary information</returns>
        /// <response code="200">Returns a list of paths</response>
        [HttpGet("deleted")]
        [HttpHead("deleted")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<DeletedPath>>> GetDeleted([FromQuery] RequestParams requestParams = null)
        {
            return requestParams is not null && requestParams.UsePaging()
                ? await GetDeletedPage(requestParams)
                : await GetAllDeleted();
        }



        /// <summary>
        /// Get path information by its Id
        /// </summary>
        /// <param name="pathId">An id of the path</param>
        /// <returns>Information of the path with modules included</returns>
        /// /// <response code="200">Returns requested path</response>
        [HttpGet("{pathId}", Name = "GetPath")]
        [HttpHead("{pathId}")]
        [AllowAnonymous]
        public async Task<ActionResult<Path>> Get(int pathId)
        {
            Path model = await Mediator.Send(new GetPathQuery { Id = pathId });
            return Ok(model);
        }

        /// <summary>
        /// Create a path
        /// </summary>
        /// <param name="command">Path object</param>
        /// <returns>Created path</returns>
        /// <response code="201">Path created successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        //TODO: adding "application/xml" causes "An error occurred while deserializing input data." error for some reason
        public async Task<ActionResult<Path>> Create([FromBody] CreatePath command)
        {
            Path model = await Mediator.Send(command);

            return CreatedAtRoute("GetPath", new { pathId = model.Id }, model);
        }

        /// <summary>
        /// Update the path with given Id
        /// </summary>
        /// <param name="pathId">An id of the path</param>
        /// <param name="command">Updated path</param>
        /// <returns>Updated path</returns>
        /// <response code="200">Path updated successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{pathId}")]
        public async Task<ActionResult<Path>> Update(int pathId,
          [FromBody] UpdatePath command)
        {
            if (pathId != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Update the part of path with given Id
        /// </summary>
        /// <param name="patchDocument"></param>
        /// <param name="pathId">An id of the path</param>
        /// <returns>Updated path</returns>
        /// <response code="200">Path updated successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        
        [HttpPatch("{pathId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Path>> Patch([FromBody] JsonPatchDocument patchDocument, [FromRoute] int pathId)
        {
            var pathPatchCommand = new PathPathCommand(pathId, patchDocument);
            return Ok(await Mediator.Send(pathPatchCommand));
        }

        /// <summary>
        /// Update the part of deleted path with given Id. Common case is restoring deleted Path
        /// </summary>
        /// <param name="patchDocument"></param>
        /// <param name="pathId">An id of the path</param>
        /// <returns>Updated path</returns>
        /// <response code="200">Path updated successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        
        [HttpPatch("deleted/{pathId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Path>> PatchDeleted([FromBody] JsonPatchDocument patchDocument, [FromRoute] int pathId)
        {
            var pathPatchCommand = new PathPathCommand(pathId, patchDocument);
            return Ok(await Mediator.Send(pathPatchCommand));
        }

        /// <summary>
        /// Delete the path with given Id
        /// </summary>
        /// <param name="pathId">An id of the path</param>
        /// <returns></returns>
        /// <response code="204">Path deleted successfully</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{pathId}")]
        public async Task<IActionResult> Delete(int pathId)
        {
            await Mediator.Send(new DeletePath { Id = pathId });

            return NoContent();
        }

        private async Task<ActionResult<IEnumerable<Path>>> GetAll()
        {
            IEnumerable<Path> model = await Mediator.Send(new GetPathListQuery());
            return Ok(model);
        }

        private async Task<ActionResult<IEnumerable<DeletedPath>>> GetAllDeleted()
        {
            IEnumerable<DeletedPath> model = await Mediator.Send(new GetDeletedPathListQuery());
            return Ok(model);
        }

        private async Task<ActionResult<IEnumerable<Path>>> GetPage(RequestParams filter)
        {
            var (paginationData, result) = await Mediator.Send(
              new GetPathListQueryPaging()
              {
                  PageNumber = filter.PageNumber,
                  PageSize = filter.PageSize
              });

            Response?.Headers?.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationData));
            return Ok(result);
        }

        private Task<ActionResult<IEnumerable<DeletedPath>>> GetDeletedPage(RequestParams filter)
        {
            //var (paginationData, result) = await Mediator.Send(
            //    new GetDeletedPathListQuery()
            //    {
            //        PageNumber = filter.PageNumber,
            //        PageSize = filter.PageSize
            //    });

            //Response?.Headers?.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationData));
            // return Ok(result);

            return null;
        }
    }
}