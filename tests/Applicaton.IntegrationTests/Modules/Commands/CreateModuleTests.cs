using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Commands
{
  using static Testing;

  public class CreateModuleTests : TestBase
  {
    [Test]
    public void ShouldRequireMinimumFields()
    {
      var command = new CreateModuleCommand();

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>();
    }

    [Test]
    public void ShouldRequirePathId()
    {
      var command = new CreateModuleCommand
      {
        Title = "Module Title",
        Description = "Module Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("PathId"))
            .And.Errors["PathId"].Should().Contain("Path Id is required.");
    }

    [Test]
    public void ShouldReturnNotFoundForNonExistingPath()
    {
      var command = new CreateModuleCommand
      {
        PathId = 999,
        Title = "New Title",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireTitle()
    {
      var command = new CreateModuleCommand
      {
        PathId = 1,
        Title = "",
        Description = "Module Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title is required.");
    }

    [Test]
    public void ShouldDisallowLongTitle()
    {
      var command = new CreateModuleCommand
      {
        PathId = 1,
        Title = "This module title is too long and exceeds one hundred characters allowed for module titles by CreateModuleCommandValidator",
        Description = "Learn how to design modern web applications using ASP.NET"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title must not exceed 100 characters.");
    }

    [Test]
    public void ShouldRequireDescription()
    {
      var command = new CreateModuleCommand
      {
        PathId = 1,
        Title = "Module Title"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Description"))
            .And.Errors["Description"].Should().Contain("Description is required.");
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      var pathId = await AddWithIdAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      await SendAsync(new CreateModuleCommand
      {
        PathId = pathId,
        Title = "Module Title",
        Description = "Module Decription"
      });

      var command = new CreateModuleCommand
      {
        PathId = pathId,
        Title = "Module Title",
        Description = "Module Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("The specified module already exists in this path.");
    }

    [Test]
    public async Task ShouldCreateModule()
    {
      var userId = await RunAsDefaultUserAsync();

      var pathId = await AddWithIdAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var command = new CreateModuleCommand
      {
        PathId = pathId,
        Title = "New Module",
        Description = "New Module Description",
        Necessity = Domain.Enums.NecessityLevel.Other,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      };

      var createdModule = await SendAsync(command);

      var module = await FindAsync<Module>(createdModule.Id);

      module.Should().NotBeNull();
      module.Title.Should().Be(command.Title);
      module.Description.Should().Be(command.Description);
      module.Necessity.Should().Be(command.Necessity);
      module.CreatedBy.Should().Be(userId);
      module.Created.Should().BeCloseTo(DateTime.Now, 10000);
    }
  }
}
