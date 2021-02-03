using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Themes.Commands.DeleteTheme;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class DeleteThemeTests : TestBase
  {
    [Test]
    public void ShouldRequireValidThemeId()
    {
      var command = new DeleteThemeCommand { Id = 99999, ModuleId = 9999 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTheme()
    {
      var moduleId = await AddWithIdAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
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

      var themeAdded = await FindAsync<Theme>(themeId);

      await SendAsync(new DeleteThemeCommand
      {
        Id = themeId,
        ModuleId = moduleId
      });

      var themeDeleted = await FindAsync<Theme>(themeId);

      themeAdded.Should().NotBeNull();
      themeDeleted.Should().BeNull();
    }
  }
}
