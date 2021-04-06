﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Modules.Commands.DeleteModule;
using DeveloperPath.Application.Modules.Commands.UpdateModule;
using DeveloperPath.Application.Modules.Queries.GetModules;
using DeveloperPath.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DeveloperPath.Web.WebAPI.Controllers
{
  public class ModulesControllerTests : TestBase
  {
    private readonly Mock<IMediator> moqMediator;
    private readonly ModuleDto sampleModule;
    private readonly IEnumerable<ModuleDto> Modules;

    public ModulesControllerTests()
    {
      sampleModule = new ModuleDto { Id = 1, Title = "Module1", Description = "Description1" };
      Modules = new List<ModuleDto>
      {
        sampleModule,
        new ModuleDto { Id = 2, Title = "Module2", Description = "Description2" }
      };

      moqMediator = new Mock<IMediator>();
      // Get all
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetModuleListQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(Modules);
      // Get one
      moqMediator
        .Setup(m => m.Send(It.IsAny<GetModuleQuery>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleModule);
      // Create
      moqMediator
        .Setup(m => m.Send(It.IsAny<CreateModuleCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleModule);
      // Update
      moqMediator
        .Setup(m => m.Send(It.IsAny<UpdateModuleCommand>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(sampleModule);
      // Delete
      moqMediator
        .Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_ReturnsAllModules()
    {
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Get(1);
      var content = GetObjectResultContent<IEnumerable<ModuleDto>>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(2, content.Count());
    }

    [Test]
    public async Task Get_ReturnsModule()
    {
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Get(1, 1);
      var content = GetObjectResultContent<ModuleDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Create_ReturnsCreatedAtRoute()
    {
      var createCommand = new CreateModuleCommand { PathId = 1, Order = 0, Title = "Create title", Description = "Create Description" };
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Create(1, createCommand);
      var content = GetObjectResultContent<ModuleDto>(result.Result);

      Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
      Assert.AreEqual("GetModule", ((CreatedAtRouteResult)result.Result).RouteName);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsUpdatedModule_WhenRequestedIdMatchesCommandId()
    {
      var updateCommand = new UpdateModuleCommand { Id = 1, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, updateCommand);
      var content = GetObjectResultContent<ModuleDto>(result.Result);

      Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
      Assert.IsNotNull(content);
      Assert.AreEqual(1, content.Id);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
    {
      var updateCommand = new UpdateModuleCommand { Id = 2, Order = 0, Title = "Update title", Description = "Update Description" };
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Update(1, 1, updateCommand);

      Assert.IsInstanceOf(typeof(BadRequestResult), result.Result);
    }

    [Test]
    public async Task Delete_ReturnsNoContent()
    {
      var controller = new ModulesController(moqMediator.Object);
      
      var result = await controller.Delete(1, 1);

      Assert.IsInstanceOf(typeof(NoContentResult), result);
    }
  }
}
