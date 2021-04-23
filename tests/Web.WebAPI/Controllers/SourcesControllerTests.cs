using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using DeveloperPath.Application.CQRS.Sources.Commands.CreateSource;
using DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource;
using DeveloperPath.Application.CQRS.Sources.Commands.UpdateSource;
using DeveloperPath.Application.CQRS.Sources.Queries.GetSources;
using DeveloperPath.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeveloperPath.Web.WebAPI.Controllers
{
    public class SourcesControllerTests : TestBase
  {
    private readonly Mock<IMediator> moqMediator;
    private readonly SourceDto sampleSource;
    private readonly IEnumerable<SourceDto> Sources;
    private readonly UpdateSourceCommand updateCommand;

    public SourcesControllerTests()
    {
      sampleSource = new SourceDto { Id = 1, ThemeId = 1, Title = "Source1", Description = "Description1", Url = "https://www.test1.com" };
      Sources = new List<SourceDto>
      {
        sampleSource,
        new SourceDto { Id = 2, ThemeId = 1, Title = "Source2", Description = "Description2", Url = "https://www.test1.com" }
      };

      updateCommand = new UpdateSourceCommand { Id = 1, PathId = 1, ModuleId = 1, ThemeId = 1, 
        Order = 0, Title = "Create title", Description = "Create Description", Url = "http://www.ww.ww" };

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
        .Setup(m => m.Send(It.IsAny<CreateSourceCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleSource);
      // Update
      moqMediator
        .Setup(m => m.Send(It.IsAny<UpdateSourceCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleSource);
      // Delete
      moqMediator
        .Setup(m => m.Send(It.IsAny<DeleteSourceCommand>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_ReturnsAllSources()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Get(1, 1, 1);
      var content = GetObjectResultContent<IEnumerable<SourceDto>>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(2, content.Count());
    }

    [Test]
    public async Task Get_ReturnsSource()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Get(1, 1, 1, 1);
      var content = GetObjectResultContent<SourceDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Create_ReturnsCreatedAtRoute()
    {
      var createCommand = new CreateSourceCommand { PathId = 1, ModuleId = 1, ThemeId = 1, 
        Order = 0, Title = "Create title", Description = "Create Description", Url = "http://www.ww.ww" };
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Create(1, 1, 1, createCommand);
      var content = GetObjectResultContent<SourceDto>(result.Result);

      Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
      Assert.AreEqual("GetSource", ((CreatedAtRouteResult)result.Result).RouteName);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsUpdatedSource_WhenRequestedIdMatchesCommandId()
    {
      var updateCommand = new UpdateSourceCommand { Id = 1, PathId = 1, ModuleId = 1, ThemeId = 1, 
        Order = 0, Title = "Create title", Description = "Create Description", Url = "http://www.ww.ww" };
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, 1, 1, updateCommand);
      var content = GetObjectResultContent<SourceDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedPathIdDoesNotMatchCommandId()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Update(2, 1, 1, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandId()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Update(1, 2, 1, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedThemeIdDoesNotMatchCommandId()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, 2, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, 1, 2, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Delete_ReturnsNoContent()
    {
      var controller = new SourcesController(moqMediator.Object);
      
      var result = await controller.Delete(1, 1, 1, 1);

      Assert.IsInstanceOf(typeof(NoContentResult), result);
    }
  }
}
