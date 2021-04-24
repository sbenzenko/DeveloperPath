using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Themes.Commands.UpdateTheme;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class UpdateThemeTests : TestBase
  {
    [Test]
    public void ShouldRequireValidThemeId()
    {
      var command = new UpdateTheme
      {
        Id = 99999,
        ModuleId = 1,
        Title = "New Title",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireValidModuleId()
    {
      var command = new UpdateTheme
      {
        Id = 1,
        ModuleId = 99999,
        Title = "New Title",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireThemeTitle()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var command = new UpdateTheme
      {
        Id = theme.Id,
        ModuleId = module.Id,
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
    public async Task ShouldRequireDescription()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var command = new UpdateTheme
      {
        Id = theme.Id,
        ModuleId = module.Id,
        Title = "New Theme",
        Description = "",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Description"))
              .And.Errors["Description"].Should().Contain("Description is required.");
    }

    [Test]
    public async Task ShouldDisallowLongTitle()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var command = new UpdateTheme
      {
        Id = theme.Id,
        Title = "This theme title is too long and exceeds two hundred characters allowed for theme titles by CreateThemeCommandValidator. And this theme title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
        Description = "New Description",
        Necessity = 0
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Title must not exceed 200 characters.");
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Other Theme",
        Description = "New Other Theme Description",
        ModuleId = module.Id
      });

      var command = new UpdateTheme
      {
        Id = theme.Id,
        ModuleId = module.Id,
        Title = "New Theme",
        Description = "Updated Description"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Theme with this title already exists in this module.");
    }

    [Test]
    public async Task ShouldUpdateTheme()
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

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        ModuleId = module.Id
      });

      var command = new UpdateTheme
      {
        PathId = path.Id,
        ModuleId = module.Id,
        Id = theme.Id,
        Title = "Updated title",
        Description = "Updated Description",
        Necessity = 0
      };

      await SendAsync(command);

      var updatedTheme = await FindAsync<Theme>(theme.Id);

      updatedTheme.Title.Should().Be(command.Title);
      updatedTheme.Description.Should().Be(command.Description);
      updatedTheme.LastModifiedBy.Should().NotBeNull();
      updatedTheme.LastModifiedBy.Should().Be(userId);
      updatedTheme.LastModified.Should().NotBeNull();
      updatedTheme.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    }
  }
}
