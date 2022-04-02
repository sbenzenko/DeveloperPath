using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.CQRS.Sources.Commands.CreateSource;
using DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource;
using DeveloperPath.Application.CQRS.Sources.Commands.UpdateSource;
using DeveloperPath.Application.CQRS.Sources.Queries.GetSources;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Web.WebAPI.Controllers;

public class SourcesControllerTests : TestBase
{
  private readonly Mock<IMediator> moqMediator;
  private readonly Theme sampleTheme;
  private readonly Source sampleSource;
  private readonly IEnumerable<Source> Sources;
  private readonly CreateSource createCommand;
  private readonly UpdateSource updateCommand;

  public SourcesControllerTests()
  {
    sampleTheme = new Theme { Id = Guid.NewGuid(), ModuleId = Guid.NewGuid(), Title = "Theme1", Description = "Description1" };
    sampleSource = new Source { Id = Guid.NewGuid(), ThemeId = sampleTheme.Id, Title = "Source1", Description = "Description1", Url = "https://www.test1.com" };
    Sources = new List<Source>
    {
      sampleSource,
      new Source { Id = Guid.NewGuid(), ThemeId = sampleTheme.Id, Title = "Source2", Description = "Description2", Url = "https://www.test1.com" }
    };
    
    createCommand = new CreateSource
    {
      ModuleId = sampleTheme.ModuleId,
      ThemeId = sampleTheme.Id,
      Order = 0,
      Title = "Create title",
      Description = "Create Description",
      Url = "http://www.ww.ww"
    };

    updateCommand = new UpdateSource
    {
      Id = sampleSource.Id,
      ModuleId = sampleTheme.ModuleId,
      ThemeId = sampleSource.ThemeId,
      Order = 0,
      Title = "Create title",
      Description = "Create Description",
      Url = "http://www.ww.ww"
    };

    moqMediator = new Mock<IMediator>();
    // Get all
    moqMediator
      .Setup(m => m.Send(It.IsAny<GetSourceListQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Sources);
    // Get one
    moqMediator
      .Setup(m => m.Send(It.IsAny<GetSourceQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(sampleSource);
    // Create
    moqMediator
      .Setup(m => m.Send(It.IsAny<CreateSource>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(sampleSource);
    // Update
    moqMediator
        .Setup(m => m.Send(It.IsAny<UpdateSource>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(sampleSource);
    // Delete
    moqMediator
      .Setup(m => m.Send(It.IsAny<DeleteSource>(), It.IsAny<CancellationToken>()));
  }

  [Test]
  public async Task Get_ReturnsAllSources()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Get(sampleTheme.ModuleId, sampleTheme.Id);
    var content = GetObjectResultContent<IEnumerable<Source>>(result.Result);

    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
    Assert.IsNotNull(content);
    Assert.AreEqual(2, content.Count());
  }

  [Test]
  public async Task Get_ReturnsSource()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Get(sampleTheme.ModuleId, sampleSource.ThemeId, sampleSource.Id);
    var content = GetObjectResultContent<Source>(result.Result);

    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
    Assert.IsNotNull(content);
    Assert.AreEqual(sampleSource.Id, content.Id);
  }

  [Test]
  public async Task Create_ReturnsCreatedAtRoute()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Create(sampleTheme.ModuleId, sampleTheme.Id, createCommand);
    var content = GetObjectResultContent<Source>(result.Result);

    Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
    Assert.AreEqual("GetSource", ((CreatedAtRouteResult)result.Result).RouteName);
    Assert.IsNotNull(content);
    Assert.AreEqual(sampleSource.Id, content.Id);
  }

  [Test]
  public async Task Create_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandModuleId()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Create(Guid.NewGuid(), sampleTheme.Id, createCommand);

    Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  }

  [Test]
  public async Task Create_ReturnsBadRequest_WhenRequestedThemeIdDoesNotMatchCommandThemeId()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Create(sampleTheme.ModuleId, Guid.NewGuid(), createCommand);

    Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  }

  //[Test]
  //public async Task Create_ReturnsBadRequest_WhenRequestedPathIdDoesNotMatchCommandId()
  //{
  //  var createCommand = new CreateSource
  //  {
  //    PathId = Guid.Empty,
  //    ModuleId = Guid.Empty,
  //    ThemeId = Guid.Empty,
  //    Order = 0,
  //    Title = "Create title",
  //    Description = "Create Description",
  //    Url = "http://www.ww.ww"
  //  };
  //  var controller = new SourcesController(moqMediator.Object);

  //  var result = await controller.Create(Guid.Empty, Guid.Empty, Guid.Empty, createCommand);

  //  Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  //}

  [Test]
  public async Task Update_ReturnsUpdatedSource_WhenRequestedIdMatchesCommandId()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Update(sampleTheme.ModuleId, sampleSource.ThemeId, sampleSource.Id, updateCommand);
    var content = GetObjectResultContent<Source>(result.Result);

    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
    Assert.IsNotNull(content);
    Assert.AreEqual(sampleSource.Id, content.Id);
  }

  [Test]
  public async Task Update_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandId()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Update(Guid.NewGuid(), sampleSource.ThemeId, sampleSource.Id, updateCommand);

    Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  }

  [Test]
  public async Task Update_ReturnsBadRequest_WhenRequestedThemeIdDoesNotMatchCommandId()
  {
    var controller = new SourcesController(moqMediator.Object);
    
    var result = await controller.Update(sampleTheme.ModuleId, Guid.NewGuid(), sampleSource.Id, updateCommand);

    Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  }

  [Test]
  public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Update(sampleTheme.ModuleId, sampleSource.ThemeId, Guid.NewGuid(), updateCommand);

    Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
  }

  [Test]
  public async Task Delete_ReturnsNoContent()
  {
    var controller = new SourcesController(moqMediator.Object);

    var result = await controller.Delete(sampleTheme.ModuleId, sampleSource.ThemeId, sampleSource.Id);

    Assert.IsInstanceOf(typeof(NoContentResult), result);
  }
}
