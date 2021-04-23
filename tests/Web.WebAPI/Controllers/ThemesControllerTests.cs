using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Shared.Dtos.Models;
using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;
using DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
using DeveloperPath.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeveloperPath.Web.WebAPI.Controllers
{
    public class ThemesControllerTests : TestBase
  {
    private readonly Mock<IMediator> moqMediator;
    private readonly ThemeDto sampleTheme;
    private readonly IEnumerable<ThemeDto> Themes;

    public ThemesControllerTests()
    {
      sampleTheme = new ThemeDto { Id = 1, ModuleId = 1, Title = "Theme1", Description = "Description1" };
      Themes = new List<ThemeDto>
      {
        sampleTheme,
        new ThemeDto { Id = 2, ModuleId = 1, Title = "Theme2", Description = "Description2" }
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
        .Setup(m => m.Send(It.IsAny<CreateThemeCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleTheme);
      // Update
      moqMediator
        .Setup(m => m.Send(It.IsAny<UpdateThemeCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleTheme);
      // Delete
      moqMediator
        .Setup(m => m.Send(It.IsAny<DeleteThemeCommand>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_ReturnsAllThemes()
    {
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Get(1, 1);
      var content = GetObjectResultContent<IEnumerable<ThemeDto>>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(2, content.Count());
    }

    [Test]
    public async Task Get_ReturnsTheme()
    {
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Get(1, 1, 1);
      var content = GetObjectResultContent<ThemeDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Create_ReturnsCreatedAtRoute()
    {
      var createCommand = new CreateThemeCommand { PathId = 1, ModuleId = 1, Order = 0, Title = "Create title", Description = "Create Description" };
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Create(1, 1, createCommand);
      var content = GetObjectResultContent<ThemeDto>(result.Result);

      Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
      Assert.AreEqual("GetTheme", ((CreatedAtRouteResult)result.Result).RouteName);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsUpdatedTheme_WhenRequestedIdMatchesCommandId()
    {
      var updateCommand = new UpdateThemeCommand { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, 1, updateCommand);
      var content = GetObjectResultContent<ThemeDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedPathIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateThemeCommand { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Update(2, 1, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedModuleIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateThemeCommand { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Update(1, 2, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateThemeCommand { Id = 1, PathId = 1, ModuleId = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, 2, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Delete_ReturnsNoContent()
    {
      var controller = new ThemesController(moqMediator.Object);
      
      var result = await controller.Delete(1, 1, 1);

      Assert.IsInstanceOf(typeof(NoContentResult), result);
    }
  }
}
