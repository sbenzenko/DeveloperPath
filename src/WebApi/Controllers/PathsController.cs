using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.DeletePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.WebApi.Extensions;
using DeveloperPath.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ClientModels;

namespace DeveloperPath.WebApi.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Route("api/paths")]
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
        public async Task<ActionResult<IEnumerable<Path>>> Get([FromQuery] RequestParams requestParams = null)
        {
            //TODO: consider adding default page size and show 1st page instead of all
            return requestParams is not null && requestParams.UsePaging()
              ? await GetPage(requestParams)
              : await GetAll();
        }

        /// <summary>
        /// Get path information by its Id
        /// </summary>
        /// <param name="pathId">An id of the path</param>
        /// <returns>Information of the path with modules included</returns>
        /// /// <response code="200">Returns requested path</response>
        [HttpGet("{pathId}", Name = "GetPath")]
        [HttpHead("{pathId}")]
        public async Task<ActionResult<Path>> Get(int pathId)
        {
            Path model = await Mediator.Send(new GetPathQuery { Id = pathId });

            return Ok(model);
        }

        ///// <summary>
        ///// Get detailed path information by its Id
        ///// </summary>
        ///// <param name="pathId">An id of the path</param>
        ///// <returns>Detailed information of the path with modules included</returns>
        //[Route("api/pathdetails")]
        //[HttpGet("{pathId}", Name = "GetPathDetails")]
        //[HttpHead("{pathId}")]
        //public async Task<ActionResult<PathViewModel>> GetDetails(int pathId)
        //{
        //  PathViewModel model = await Mediator.Send(new GetPathDetailsQuery { Id = pathId });

        //  return Ok(model);
        //}

        /// <summary>
        /// Create a path
        /// </summary>
        /// <param name="command">Path object</param>
        /// <returns>Created path</returns>
        /// <response code="201">Path created successfully</response>
        /// <response code="406">Not acceptable entity provided</response>
        /// <response code="415">Unsupported media type provided</response>
        /// <response code="422">Unprocessible entity provided</response>
        [HttpPost]
        //TODO: adding "application/xml" causes "An error occurred while deserializing input data." error for some reason
        [Consumes("application/json")]
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
        [HttpPut("{pathId}")]
        [Consumes("application/json")]
        public async Task<ActionResult<Path>> Update(int pathId,
          [FromBody] UpdatePath command)
        {
            if (pathId != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        // TODO: add PATCH

        /// <summary>
        /// Delete the path with given Id
        /// </summary>
        /// <param name="pathId">An id of the path</param>
        /// <returns></returns>
        /// <response code="204">Path deleted successfully</response>
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
    }
}