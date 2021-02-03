using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Themes.Queries.GetTheme;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Queries
{
  using static Testing;

  public class GetThemeTests : TestBase
  {
    [Test]
    public async Task ShouldReturnTheme()
    {
      var moduleId = await AddWithIdAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Themes = new List<Theme> { },
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
        ModuleId = moduleId,
        Description = "New Theme Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Complexity = Domain.Enums.ComplexityLevel.Beginner,
        Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
        Order = 2,
        Sources = new List<Source>
        {
          new Source {
              Title = "Source1",
              Type = Domain.Enums.SourceType.Blog,
              Url = "https://www.google.com",
              Availability = Domain.Enums.AvailabilityLevel.Free },
          new Source {
              Title = "Source2",
              Type = Domain.Enums.SourceType.Blog,
              Url = "https://www.microsoft.com",
              Availability = Domain.Enums.AvailabilityLevel.RequiresRegistration }
        },
        Section = new Section
        {
          ModuleId = moduleId,
          Title = "First Section"
        }
      });

      var query = new GetThemeQuery() { Id = themeId, ModuleId = moduleId };

      var theme = await SendAsync(query);

      theme.Id.Should().Be(themeId);
      theme.Title.Should().NotBeEmpty();
      theme.Description.Should().NotBeEmpty();
      theme.Section.Should().NotBeNull();
      theme.Module.Should().NotBeNull();
      theme.Module.Id.Should().Be(moduleId);
      theme.Sources.Should().HaveCount(2);
      theme.Tags.Should().HaveCount(3);
    }
  }
}
