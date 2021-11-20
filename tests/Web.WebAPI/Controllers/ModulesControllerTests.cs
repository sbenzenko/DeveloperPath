using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Shared.ClientModels;
using DeveloperPath.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Web.WebAPI.Controllers
{
    public class ModulesControllerTests : TestBase
    {
        private readonly Mock<IMediator> moqMediator;
        private readonly Module sampleModule;
        private readonly IEnumerable<Module> modules;
        public ModulesControllerTests()
        {
            sampleModule = new Module { Id = 1, Title = "Module1", Description = "Description1" };
            modules = new List<Module>
      {
        sampleModule,
        new Module { Id = 2, Title = "Module2", Description = "Description2" }
      };

            moqMediator = new Mock<IMediator>();
            // Get all
            moqMediator
              .Setup(m => m.Send(It.IsAny<GetModuleListQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(modules);
            // Get 1st page
            moqMediator
              .Setup(m => m.Send(It.IsAny<GetModuleListQueryPaging>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync((new PaginationData(1, 1), modules.Take(1)));
            // Get one
            moqMediator
              .Setup(m => m.Send(It.IsAny<GetModuleQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(sampleModule);
            // Create
            moqMediator
              .Setup(m => m.Send(It.IsAny<CreateModule>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(sampleModule);
            // Update
            moqMediator
              .Setup(m => m.Send(It.IsAny<UpdateModule>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(sampleModule);
            // Delete
            moqMediator
              .Setup(m => m.Send(It.IsAny<DeleteModule>(), It.IsAny<CancellationToken>()));
        }

        //[Test]
        //public async Task Get_ReturnsAllModules()
        //{
        //    var controller = new ModulesController(moqMediator.Object);

        //    var result = await controller.Get("asp-net-developer");
        //    var content = GetObjectResultContent<IEnumerable<Module>>(result.Result);

        //    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        //    Assert.IsNotNull(content);
        //    Assert.AreEqual(2, content.Count());
        //}

        //[Test]
        //public async Task Get_ReturnsModule()
        //{
        //    var controller = new ModulesController(moqMediator.Object);

        //    var result = await controller.Get("asp-net-developer", 1);
        //    var content = GetObjectResultContent<Module>(result.Result);

        //    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        //    Assert.IsNotNull(content);
        //    Assert.AreEqual(1, content.Id);
        //}

        //[Test]
        //public async Task Get_ReturnsAllModules_WhenNoPagingProvided()
        //{
        //    var controller = new ModulesController(moqMediator.Object);

        //    var result = await controller.Get("asp-net-developer", null);

        //    var contentResult = (OkObjectResult)result.Result;
        //    var value = contentResult.Value as IEnumerable<Module>;

        //    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        //    Assert.IsNotNull(contentResult);
        //    Assert.AreEqual(2, value.Count());
        //}

        //[Test]
        //public async Task Get_ReturnsModulesPage_WhenPagingProvided()
        //{
        //    var controller = new ModulesController(moqMediator.Object);
        //    var result = await controller.Get("asp-net-developer", new RequestParams()
        //    {
        //        PageNumber = 1,
        //        PageSize = 1
        //    });

        //    var contentResult = (OkObjectResult)result.Result;
        //    var value = contentResult.Value as IEnumerable<Module>;

        //    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        //    Assert.IsNotNull(contentResult);
        //    Assert.AreEqual(1, value.Count());
        //}

        //[Test]
        //public async Task Get_ReturnsAllModules_WhenPagingIsNotValid()
        //{
        //    var controller = new ModulesController(moqMediator.Object);
        //    var result = await controller.Get("asp-net-developer", new RequestParams()
        //    {
        //        PageNumber = -1,
        //        PageSize = -1
        //    });

        //    var contentResult = (OkObjectResult)result.Result;
        //    var value = contentResult.Value as IEnumerable<Module>;

        //    Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        //    Assert.IsNotNull(contentResult);
        //    Assert.AreEqual(2, value.Count());
        //}

        [Test]
        public async Task Create_ReturnsCreatedAtRoute()
        {
            var createCommand = new CreateModule { Order = 0, Title = "Create title", Description = "Create Description" };
            var controller = new ModulesController(moqMediator.Object);

            var result = await controller.Create(createCommand);
            var content = GetObjectResultContent<Module>(result.Result);

            Assert.IsInstanceOf(typeof(CreatedAtRouteResult), result.Result);
            Assert.AreEqual("GetModule", ((CreatedAtRouteResult)result.Result).RouteName);
            Assert.IsNotNull(content);
            Assert.AreEqual(1, content.Id);
        }

        [Test]
        public async Task Update_ReturnsUpdatedModule_WhenRequestedIdMatchesCommandId()
        {
            var updateCommand = new UpdateModule { Id = 1, Order = 0, Title = "Update title", Description = "Update Description" };
            var controller = new ModulesController(moqMediator.Object);

            var result = await controller.Update(1, updateCommand);
            var content = GetObjectResultContent<Module>(result.Result);

            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
            Assert.IsNotNull(content);
            Assert.AreEqual(1, content.Id);
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenRequestedIdDoesNotMatchCommandId()
        {
            var updateCommand = new UpdateModule { Id = 2, Order = 0, Title = "Update title", Description = "Update Description" };
            var controller = new ModulesController(moqMediator.Object);

            var result = await controller.Update(1, updateCommand);

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