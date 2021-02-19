using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Modules.Commands.UpdateModule;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class UpdateModuleTests : TestBase
  {
    [Test]
    public void ShouldRequireValidModuleId()
    {
      var command = new UpdateModuleCommand
      {
        Id = 99,
        Title = "New Title",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireModuleTitle()
    {
      var path = await SendAsync(new CreatePathCommand
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description"
      });

      var command = new UpdateModuleCommand
      {
        Id = module.Id,
        Title = "",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Title is required.");
    }

    [Test]
    public async Task ShouldRequireModuleDescription()
    {
      var path = await SendAsync(new CreatePathCommand
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description"
      });

      var command = new UpdateModuleCommand
      {
        Id = module.Id,
        Title = "Updated Module",
        Description = "",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Description"))
              .And.Errors["Description"].Should().Contain("Description is required.");
    }

    [Test]
    public async Task ShouldDisallowLongModuleTitle()
    {
      var path = await SendAsync(new CreatePathCommand
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description"
      });

      var command = new UpdateModuleCommand
      {
        Id = module.Id,
        Title = "This module title is too long and exceeds one hundred characters allowed for module titles by UpdateModuleCommandValidator",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Title must not exceed 100 characters.");
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      var path = await AddAsync(new Path
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description"
      });

      await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "Other New Module",
        Description = "New Other Path Description"
      });

      var command = new UpdateModuleCommand
      {
        Id = module.Id,
        Title = "Other New Module",
        Description = "New Module Description"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Module with this title already exists in one of associated paths.");
    }

    [Test]
    public async Task ShouldUpdateModule()
    {
      var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description"
      });

      var command = new UpdateModuleCommand
      {
        Id = module.Id,
        Title = "Updated title",
        Description = "Updated Description",
        Necessity = 0
      };

      await SendAsync(command);

      var updatedModule = await FindAsync<Module>(module.Id);

      updatedModule.Title.Should().Be(command.Title);
      updatedModule.Description.Should().Be(command.Description);
      updatedModule.LastModifiedBy.Should().NotBeNull();
      updatedModule.LastModifiedBy.Should().Be(userId);
      updatedModule.LastModified.Should().NotBeNull();
      updatedModule.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    }
  }
}
