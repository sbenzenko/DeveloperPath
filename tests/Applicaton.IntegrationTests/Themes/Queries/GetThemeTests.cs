using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Themes.Queries.GetThemes;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Queries
{
  using static Testing;

  public class GetThemeTests : TestBase
  {

    [Test]
    public async Task ShouldReturnThemesList()
    {
      var path = await AddAsync(
        new Path { Title = "Some Path", Description = "Some Path Description" });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Other Module",
        Description = "New Other Module Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });
      await AddAsync(new Theme
      {
        Title = "New Theme1",
        ModuleId = module.Id,
        Description = "New Theme1 Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Complexity = Domain.Enums.ComplexityLevel.Beginner,
        Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
        Order = 1
      });
      await AddAsync(new Theme
      {
        Title = "New Theme2",
        ModuleId = module.Id,
        Description = "New Theme2 Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Complexity = Domain.Enums.ComplexityLevel.Beginner,
        Tags = new List<string> { "Theme2", "ThemeTag2", "Tag3" },
        Order = 2
      });
      await AddAsync(new Theme
      {
        Title = "New Theme3",
        ModuleId = module.Id,
        Description = "New Theme3 Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Complexity = Domain.Enums.ComplexityLevel.Beginner,
        Tags = new List<string> { "Theme2", "ThemeTag2", "Tag3" },
        Order = 3
      });

      var query = new GetThemeListQuery{ PathId = path.Id, ModuleId = module.Id };

      var result = await SendAsync(query);

      result.Should().HaveCount(3);
      (result.ToList())[2].Title.Should().Be("New Theme3");
    }

    [Test]
    public async Task ShouldReturnTheme()
    {
      var path = await AddAsync(
        new Path { Title = "Some Path", Description = "Some Path Description" });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "New Module Module",
        Description = "New Module Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        ModuleId = module.Id,
        Description = "New Theme Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Complexity = Domain.Enums.ComplexityLevel.Beginner,
        Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
        Order = 2,
        Section = new Section
        {
          ModuleId = module.Id,
          Title = "First Section"
        }
      });

      var query = new GetThemeQuery() { PathId = path.Id, ModuleId = module.Id, Id = theme.Id};

      var createdTheme = await SendAsync(query);

      createdTheme.Id.Should().Be(theme.Id);
      createdTheme.Title.Should().NotBeEmpty();
      createdTheme.Description.Should().NotBeEmpty();
      createdTheme.Section.Should().NotBeNull();
      createdTheme.ModuleId.Should().Be(module.Id);
      createdTheme.Tags.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldReturnThemeDetails()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Other Module",
        Description = "New Other Module Description",
        Necessity = Domain.Enums.NecessityLevel.MustKnow,
        Themes = new List<Theme> { },
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Other Theme",
        ModuleId = module.Id,
        Description = "New Other Theme Description",
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
          ModuleId = module.Id,
          Title = "First Section"
        }
      });

      var query = new GetThemeDetailsQuery() { Id = theme.Id, ModuleId = module.Id };

      var createdTheme = await SendAsync(query);

      createdTheme.Id.Should().Be(theme.Id);
      createdTheme.Title.Should().NotBeEmpty();
      createdTheme.Description.Should().NotBeEmpty();
      createdTheme.Section.Should().NotBeNull();
      createdTheme.Module.Should().NotBeNull();
      createdTheme.Module.Id.Should().Be(module.Id);
      createdTheme.Sources.Should().HaveCount(2);
      createdTheme.Tags.Should().HaveCount(3);
    }
  }
}