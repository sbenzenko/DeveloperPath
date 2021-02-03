using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Modules.Queries.GetModules;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Queries
{
  using static Testing;

  public class GetModuleTests : TestBase
  {
    [Test]
    public async Task ShouldReturnModuleWithThemes()
    {
      var moduleId = await AddWithIdAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Themes = new List<Theme>
          {
            new Theme { Title = "Theme1", Description = "Theme 1 Description" },
            new Theme { Title = "Theme2", Description = "Theme 2 Description" },
            new Theme { Title = "Theme3", Description = "Theme 3 Description" },
            new Theme { Title = "Theme4", Description = "Theme 4 Description" },
            new Theme { Title = "Theme5", Description = "Theme 5 Description" }
          },
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var query = new GetModuleQuery() { Id = moduleId };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Description.Should().NotBeEmpty();
      result.Themes.Should().HaveCount(5);
      result.Tags.Should().HaveCount(3);
    }
  }
}
