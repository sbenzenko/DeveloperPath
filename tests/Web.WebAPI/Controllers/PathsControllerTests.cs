using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Paging;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.DeletePath;
using DeveloperPath.Application.Paths.Commands.UpdatePath;
using DeveloperPath.Application.Paths.Queries.GetPaths;
using DeveloperPath.WebApi.Controllers;
using DeveloperPath.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeveloperPath.Web.WebAPI.Controllers
{
  public class PathsControllerTests : TestBase
  {
    private readonly Mock<IMediator> moqMediator;
    private readonly PathDto samplePath;
    private readonly IEnumerable<PathDto> paths;

    public PathsControllerTests()
    {
      samplePath = new PathDto { Id = 1, Title = "Path1", Description = "Description1" };
      paths = new List<PathDto>
      {
        samplePath,
        new PathDto { Id = 2, Title = "Path2", Description = "Description2" }
      };

      moqMediator = new Mock<IMediator>();
      // Get all
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetPathListQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(paths);
      // Get 1st page
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetPathListQueryPaging>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((new PaginationData(1, 1), paths.Take(1)));
      // Get one
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetPathQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(samplePath);
      // Create
      moqMediator
        .Setup(m => m.Send(It.IsAny<CreatePathCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(samplePath);
      // Update
      moqMediator
        .Setup(m => m.Send(It.IsAny<UpdatePathCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(samplePath);
      // Delete
      moqMediator
        .Setup(m => m.Send(It.IsAny<DeletePathCommand>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_ReturnsAllPaths()
    {
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Get();
      var content = GetObjectResultContent<IEnumerable<PathDto>>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(2, content.Count());
    }

    [Test]
    public async Task Get_ReturnsPath()
    {
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Get(1);
      var content = GetObjectResultContent<PathDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Get_ReturnsAllPaths_WhenNoPagingProvided()
    {
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Get(null);
      var contentResult = (OkObjectResult)result.Result;
      var value = contentResult.Value as IEnumerable<PathDto>;

      Assert.IsInstanceOf<OkObjectResult>(result.Result);
      Assert.IsNotNull(contentResult);
      Assert.AreEqual(2, value.Count());
    }

    [Test]
    public async Task Get_ReturnsPathsPage_WhenPagingProvided()
    {
      var controller = new PathsController(moqMediator.Object);
      var result = await controller.Get(new RequestParams()
      {
        PageNumber = 1,
        PageSize = 1
      });

      var contentResult = (OkObjectResult)result.Result;
      var value = contentResult.Value as IEnumerable<PathDto>;

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(contentResult);
      Assert.AreEqual(1, value.Count());
    }

    public async Task Get_ReturnsAllPaths_WhenPagingIsNotValid()
    {
      var controller = new PathsController(moqMediator.Object);
      var result = await controller.Get(new RequestParams() { PageNumber = -1, PageSize = -1 });
      var contentResult = (BadRequestObjectResult)result.Result;
      var value = contentResult.Value as IEnumerable<PathDto>;

      Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
      Assert.IsNotNull(contentResult);
      Assert.AreEqual(2, value.Count());
    }

    [Test]
    public async Task Create_ReturnsCreatedAtRoute()
    {
      var createCommand = new CreatePathCommand { Title = "Create title", Description = "Create Description" };
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Create(createCommand);
      var content = GetObjectResultContent<PathDto>(result.Result);

      Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
      Assert.AreEqual("GetPath", ((CreatedAtRouteResult)result.Result).RouteName);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsUpdatedPath_WhenRequestedIdMatchesCommandId()
    {
      var updateCommand = new UpdatePathCommand { Id = 1, Title = "Update title", Description = "Update Description" };
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Update(1, updateCommand);
      var content = GetObjectResultContent<PathDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdatePathCommand { Id = 2, Title = "Update title", Description = "Update Description" };
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Update(1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Delete_ReturnsNoContent()
    {
      var controller = new PathsController(moqMediator.Object);

      var result = await controller.Delete(1);

      Assert.IsInstanceOf(typeof(NoContentResult), result);
    }
  }
}