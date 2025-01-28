using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Controllers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace Web.WebAPI.Controllers
{
  public class ThemesControllerTests : TestBase
  {
    private readonly Mock<IMediator> moqMediator;
    private readonly Theme sampleTheme;
    private readonly IEnumerable<Theme> Themes;

    public ThemesControllerTests()
    {
      sampleTheme = new Theme { Id = 1, ModuleId = 1, Title = "Theme1", Description = "Description1" };
      Themes = new List<Theme>
      {
        sampleTheme,
        new Theme { Id = 2, ModuleId = 1, Title = "Theme2", Description = "Description2" }
      };

      moqMediator = new Mock<IMediator>();
      // Get all
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetThemeListQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(Themes);
      // Get one
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetThemeQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleTheme);
      // Create
      moqMediator
        .Setup(m => m.Send(It.IsAny<CreateTheme>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleTheme);
      // Update
      moqMediator
        .Setup(m => m.Send(It.IsAny<UpdateTheme>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleTheme);
      // Delete
      moqMediator
        .Setup(m => m.Send(It.IsAny<DeleteTheme>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_ReturnsAllThemes()
    {
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Get(1, 1);
      var content = GetObjectResultContent<IEnumerable<Theme>>(result.Result);

      Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
      Assert.That(content, Is.Not.Null);
      Assert.Equals(2, content.Count());
    }

    [Test]
    public async Task Get_ReturnsTheme()
    {
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Get(1, 1, 1);
      var content = GetObjectResultContent<Theme>(result.Result);

      Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
      Assert.That(content, Is.Not.Null);
      Assert.Equals(1, content.Id);
    }

    [Test]
    public async Task Create_ReturnsCreatedAtRoute()
    {
      var createCommand = new CreateTheme { PathId = 1, ModuleId = 1, Order = 0, Title = "Create title", Description = "Create Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Create(1, 1, createCommand);
      var content = GetObjectResultContent<Theme>(result.Result);

      Assert.That(result.Result, Is.InstanceOf<CreatedAtRouteResult>());
      Assert.Equals("GetTheme", ((CreatedAtRouteResult)result.Result).RouteName);
      Assert.That(content, Is.Not.Null);
      Assert.Equals(1, content.Id);
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenRequestedPathIdDoesNotMatchCommandId()
    {
      var createCommand = new CreateTheme { PathId = 1, ModuleId = 1, Order = 0, Title = "Create title", Description = "Create Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Create(2, 1, createCommand);

      Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Create_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandModuleId()
    {
      var createCommand = new CreateTheme { PathId = 1, ModuleId = 1, Order = 0, Title = "Create title", Description = "Create Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Create(1, 2, createCommand);

      Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Update_ReturnsUpdatedTheme_WhenRequestedIdMatchesCommandId()
    {
      var updateCommand = new UpdateTheme { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Update(1, 1, 1, updateCommand);
      var content = GetObjectResultContent<Theme>(result.Result);

      Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
      Assert.That(content, Is.Not.Null);
      Assert.Equals(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedPathIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateTheme { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Update(2, 1, 1, updateCommand);

      Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateTheme { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Update(1, 2, 1, updateCommand);

      Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateTheme { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Update(1, 1, 2, updateCommand);

      Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public async Task Delete_ReturnsNoContent()
    {
      var controller = new ThemesController(moqMediator.Object);

      var result = await controller.Delete(1, 1, 1);

      Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
  }
}
