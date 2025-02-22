﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.DeletePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Controllers;
using DeveloperPath.WebApi.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace Web.WebAPI.Controllers;

public class PathsControllerTests : TestBase
{
  private readonly Mock<IMediator> moqMediator;
  private readonly Path samplePath;
  private readonly PathDetails sampleDetailsPath;
  private readonly IEnumerable<Path> paths;

  public PathsControllerTests()
  {
    samplePath = new Path { Id = 1, Title = "Path1", Description = "Description1" };
    sampleDetailsPath = new PathDetails { Id = 1, Title = "Path1", Description = "Description1" };
    paths =
    [
      samplePath,
      new() { Id = 2, Title = "Path2", Description = "Description2" },
      new() { Id = 3, Title = "Path3", Description = "Description3" },
      new() { Id = 4, Title = "Path4", Description = "Description4" },
      new() { Id = 5, Title = "Path5", Description = "Description5" }
    ];

    moqMediator = new Mock<IMediator>();
    // Get all
    moqMediator
      .Setup(m => m.Send(It.IsAny<GetPathListQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(paths);
    // Get 1st page
    //moqMediator
    //  .Setup(m => m.Send(It.IsAny<GetPathListQueryPaging>(), It.IsAny<CancellationToken>()))
    //  .ReturnsAsync((new PaginationData(1, 1), paths.Take(1)));
    // Get one
    moqMediator
      .Setup(m => m.Send(It.IsAny<GetPathQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(samplePath);
    // Get details
    moqMediator
      .Setup(m => m.Send(It.IsAny<GetPathDetailsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(sampleDetailsPath);
    // Create
    moqMediator
      .Setup(m => m.Send(It.IsAny<CreatePath>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(samplePath);
    // Update
    moqMediator
      .Setup(m => m.Send(It.IsAny<UpdatePath>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(samplePath);
    // Delete
    moqMediator
      .Setup(m => m.Send(It.IsAny<DeletePath>(), It.IsAny<CancellationToken>()));
  }

  //[Test]
  //public async Task Get_ReturnsAllPaths()
  //{
  //  var controller = new PathsController(moqMediator.Object);

  //  var result = await controller.Get();
  //  var content = GetObjectResultContent<IEnumerable<Path>>(result.Result);

  //  Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
  //  Assert.IsNotNull(content);
  //  Assert.AreEqual(2, content.Count());
  //}

  [Test]
  public async Task Get_ReturnsPath()
  {
    var controller = new PathsController(moqMediator.Object);

    var result = await controller.Get(1);
    var content = GetObjectResultContent<PathDetails>(result.Result);

    Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    Assert.That(content, Is.Not.Null);
    Assert.That(content.Id, Is.EqualTo(1));
  }

  //[Test]
  //public async Task Get_ReturnsAllPaths_WhenNoPagingProvided()
  //{
  //  var controller = new PathsController(moqMediator.Object);

  //  var result = await controller.Get(null);
  //  var contentResult = (OkObjectResult)result.Result;
  //  var value = contentResult.Value as IEnumerable<Path>;

  //  Assert.IsInstanceOf<OkObjectResult>(result.Result);
  //  Assert.IsNotNull(contentResult);
  //  Assert.AreEqual(2, value.Count());
  //}

  //[Test]
  //public async Task Get_ReturnsAllPaths_WhenInvalidPageNumberProvided()
  //{
  //  var controller = new PathsController(moqMediator.Object);

  //  var result = await controller.Get(new PathRequestParams()
  //  {
  //    PageNumber = 0,
  //    PageSize = 1
  //  });
  //  var contentResult = (OkObjectResult)result.Result;
  //  var value = contentResult.Value as IEnumerable<Path>;

  //  Assert.IsInstanceOf<OkObjectResult>(result.Result);
  //  Assert.IsNotNull(contentResult);
  //  Assert.AreEqual(2, value.Count());
  //}

  //[Test]
  //public async Task Get_ReturnsAllPaths_WhenInvalidPageSizeProvided()
  //{
  //  var controller = new PathsController(moqMediator.Object);

  //  var result = await controller.Get(new PathRequestParams()
  //  {
  //    PageNumber = 1,
  //    PageSize = 0
  //  });
  //  var contentResult = (OkObjectResult)result.Result;
  //  var value = contentResult.Value as IEnumerable<Path>;

  //  Assert.IsInstanceOf<OkObjectResult>(result.Result);
  //  Assert.IsNotNull(contentResult);
  //  Assert.AreEqual(2, value.Count());
  //}

  //[Test]
  //public async Task Get_ReturnsPathsPage_WhenPagingProvided()
  //{
  //  var controller = new PathsController(moqMediator.Object);
  //  var result = await controller.Get(new PathRequestParams()
  //  {
  //    PageNumber = 1,
  //    PageSize = 1
  //  });

  //  var contentResult = (OkObjectResult)result.Result;
  //  var value = contentResult.Value as IEnumerable<Path>;

  //  Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
  //  Assert.IsNotNull(contentResult);
  //  Assert.AreEqual(1, value.Count());
  //}

  public async Task Get_ReturnsAllPaths_WhenPagingIsNotValid()
  {
    var controller = new PathsController(moqMediator.Object);
    var result = await controller.Get(new PathRequestParams() { PageNumber = -1, PageSize = -1 });
    var contentResult = (BadRequestObjectResult)result.Result;
    var value = contentResult.Value as IEnumerable<Path>;

    Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    Assert.That(contentResult, Is.Not.Null);
    Assert.That(value.Count(), Is.EqualTo(2));
  }

  [Test]
  public async Task Create_ReturnsCreatedAtRoute()
  {
    var createCommand = new CreatePath { Title = "Create title", Description = "Create Description" };
    var controller = new PathsController(moqMediator.Object);

    var result = await controller.Create(createCommand);
    var content = GetObjectResultContent<Path>(result.Result);

    Assert.That(result.Result, Is.InstanceOf<CreatedAtRouteResult>());
    Assert.That(((CreatedAtRouteResult)result.Result).RouteName, Is.EqualTo("GetPath"));
    Assert.That(content, Is.Not.Null);
    Assert.That(content.Id, Is.EqualTo(1));
  }

  [Test]
  public async Task Update_ReturnsUpdatedPath_WhenRequestedIdMatchesCommandId()
  {
    var updateCommand = new UpdatePath { Id = 1, Title = "Update title", Description = "Update Description" };
    var controller = new PathsController(moqMediator.Object);

    var result = await controller.Update(1, updateCommand);
    var content = GetObjectResultContent<Path>(result.Result);

    Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    Assert.That(content, Is.Not.Null);
    Assert.That(content.Id, Is.EqualTo(1));
  }

  [Test]
  public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
  {
    var updateCommand = new UpdatePath { Id = 2, Title = "Update title", Description = "Update Description" };
    var controller = new PathsController(moqMediator.Object);

    var result = await controller.Update(1, updateCommand);

    Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
  }

  [Test]
  public async Task Delete_ReturnsNoContent()
  {
    var controller = new PathsController(moqMediator.Object);

    var result = await controller.Delete(1);

    Assert.That(result, Is.InstanceOf<NoContentResult>());
  }
}