using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class CreateThemeTests : TestBase
  {
    [Test]
    public void ShouldRequireMinimumFields()
    {
      var command = new CreateTheme();

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>();
    }

    [Test]
    public void ShouldRequireModuleId()
    {
      var command = new CreateTheme
      {
        PathId = 1,
        Title = "Theme Title",
        Description = "Theme Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("ModuleId"))
            .And.Errors["ModuleId"].Should().Contain("Module Id is required.");
    }

    [Test]
    public void ShouldReturnNotFoundForNonExistingModule()
    {
      var command = new CreateTheme
      {
        ModuleId = 999,
        Title = "Theme Title",
        Description = "Theme Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireTitle()
    {
      var command = new CreateTheme
      {
        ModuleId = 1,
        Title = "",
        Description = "Theme Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title is required.");
    }

    [Test]
    public void ShouldDisallowLongTitle()
    {
      var command = new CreateTheme
      {
        ModuleId = 1,
        Title = "This theme title is too long and exceeds two hundred characters allowed for theme titles by CreateThemeCommandValidator. And this theme title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
        Description = "Theme Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title must not exceed 200 characters.");
    }

    [Test]
    public void ShouldRequireDescription()
    {
      var command = new CreateTheme
      {
        ModuleId = 1,
        Title = "Theme Title"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Description"))
            .And.Errors["Description"].Should().Contain("Description is required.");
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
      });

      await SendAsync(new CreateTheme
      {
        PathId = path.Id,
        ModuleId = module.Id,
        Title = "Theme Title",
        Description = "Theme Decription"
      });

      var command = new CreateTheme
      {
        ModuleId = module.Id,
        Title = "Theme Title",
        Description = "Theme Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("The specified theme already exists in this module.");
    }

    [Test]
    public async Task ShouldCreateThemeWithoutSection()
    {
      var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
      });

      var command = new CreateTheme
      {
        PathId = path.Id,
        ModuleId = module.Id,
        Title = "New Theme",
        Description = "New Theme Description",
        Necessity = Necessity.Other,
        Complexity = Complexity.Beginner,
        Order = 1
      };

      var createdTheme = await SendAsync(command);

      var theme = await FindAsync<Theme>(createdTheme.Id);

      theme.Should().NotBeNull();
      theme.Title.Should().Be(command.Title);
      theme.Description.Should().Be(command.Description);
      theme.Necessity.Should().Be(command.Necessity);
      theme.CreatedBy.Should().Be(userId);
      theme.Created.Should().BeCloseTo(DateTime.Now, 10000);
    }

    [Test]
    public async Task ShouldCreateThemeWithExistingSection()
    {
      //var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
        {
          Title = "Some Path",
          Description = "Some Path Description"
        });

      var module = await SendAsync(new CreateModule
        {
          PathId = path.Id,
          Title = "Module Title",
          Description = "Module Decription"
        });

      var sect = await AddAsync( new Section
        {
            ModuleId = module.Id,
            Title = "First Section"
        });

      var command = new CreateTheme
      {
        PathId = path.Id,
        ModuleId = module.Id,
        Title = "New Theme",
        Description = "New Theme Description",
        Necessity = Necessity.Other,
        Complexity = Complexity.Beginner,
        Order = 1,
        SectionId = sect.Id
      };

      var createdTheme = await SendAsync(command);

      var theme = await FindAsync<Theme>(createdTheme.Id);

      theme.Should().NotBeNull();
      theme.Title.Should().Be(command.Title);
      theme.Description.Should().Be(command.Description);
      theme.ModuleId.Should().Be(module.Id);
      theme.Necessity.Should().Be(command.Necessity);
      //theme.CreatedBy.Should().Be(userId);
      theme.Created.Should().BeCloseTo(DateTime.Now, 10000);
    }

    [Test]
    public async Task ShouldReturnNotfoundForNonExistingSection()
    {
      //var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
      });

      var command = new CreateTheme
      {
        ModuleId = module.Id,
        Title = "New Theme",
        Description = "New Theme Description",
        Necessity = Necessity.Other,
        Complexity = Complexity.Beginner,
        Order = 1,
        SectionId = 999
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }
  }
}
