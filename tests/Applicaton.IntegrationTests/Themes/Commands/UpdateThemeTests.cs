using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
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
      var command = new UpdateThemeCommand
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
      var command = new UpdateThemeCommand
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
      var moduleId = await AddWithIdAsync(new Module
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

      var themeId = await AddWithIdAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = moduleId
      });

      var command = new UpdateThemeCommand
      {
        Id = themeId,
        ModuleId = moduleId,
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
      var moduleId = await AddWithIdAsync(new Module
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

      var themeId = await AddWithIdAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = moduleId
      });

      var command = new UpdateThemeCommand
      {
        Id = themeId,
        ModuleId = moduleId,
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
    public async Task ShouldDisallowLongModuleTitle()
    {
      var moduleId = await AddWithIdAsync(new Module
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

      var themeId = await AddWithIdAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = moduleId
      });

      var command = new UpdateThemeCommand
      {
        Id = moduleId,
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
      var moduleId = await AddWithIdAsync(new Module
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
        ModuleId = moduleId
      });

      var themeId = await AddWithIdAsync(new Theme
      {
        Title = "New Other Theme",
        Description = "New Other Theme Description",
        ModuleId = moduleId
      });

      var command = new UpdateThemeCommand
      {
        Id = themeId,
        ModuleId = moduleId,
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

      var moduleId = await AddWithIdAsync(new Module
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

      var themeId = await AddWithIdAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        ModuleId = moduleId
      });

      var command = new UpdateThemeCommand
      {
        Id = themeId,
        ModuleId = moduleId,
        Title = "Updated title",
        Description = "Updated Description",
        Necessity = 0
      };

      await SendAsync(command);

      var updatedTheme = await FindAsync<Theme>(themeId);

      updatedTheme.Title.Should().Be(command.Title);
      updatedTheme.Description.Should().Be(command.Description);
      updatedTheme.LastModifiedBy.Should().NotBeNull();
      updatedTheme.LastModifiedBy.Should().Be(userId);
      updatedTheme.LastModified.Should().NotBeNull();
      updatedTheme.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    }
  }
}
