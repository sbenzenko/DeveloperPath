using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
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
    public async Task ShouldReturnModuleList()
    {
      var path = await AddAsync(
        new Path { Title = "Some Path", Description = "Some Path Description" });

      _ = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "New Module1",
        Description = "New Module1 Description"
      });
      _ = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "New Module2",
        Description = "New Module2 Description"
      });
      _ = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "New Module3",
        Description = "New Module3 Description"
      });

      var query = new GetModuleListQuery { PathId = path.Id };
      var result = await SendAsync(query);

      result.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldReturnModule()
    {
      var path = await AddAsync(
        new Path { Title = "Some Other Path", Description = "Some Path Description" });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "New Module",
        Description = "New Module Description",
        Necessity = Domain.Enums.Necessity.MustKnow,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var query = new GetModuleQuery() { PathId = path.Id, Id = module.Id };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Description.Should().NotBeEmpty();
      result.Tags.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldReturnModuleDetails()
    {
      var path = await AddAsync(
        new Path { Title = "Some Another Path", Description = "Path Description" });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "New Other Module",
        Description = "New Other Module Description",
        Necessity = Domain.Enums.Necessity.MustKnow,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });
      await AddAsync(new Theme
      {
        Title = "New Theme1",
        ModuleId = module.Id,
        Description = "New Theme1 Description",
        Necessity = Domain.Enums.Necessity.MustKnow,
        Complexity = Domain.Enums.Complexity.Beginner,
        Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
        Order = 1
      });
      await AddAsync(new Theme
      {
        Title = "New Theme2",
        ModuleId = module.Id,
        Description = "New Theme2 Description",
        Necessity = Domain.Enums.Necessity.MustKnow,
        Complexity = Domain.Enums.Complexity.Beginner,
        Tags = new List<string> { "Theme2", "ThemeTag2", "Tag3" },
        Order = 2
      });

      var query = new GetModuleDetailsQuery() { PathId = path.Id, Id = module.Id };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Description.Should().NotBeEmpty();
      result.Themes.Should().HaveCount(2);
      result.Tags.Should().HaveCount(3);
    }

    [Test]
    public void ListShouldReturnNotFound_WhenPathIdNotFound()
    {
      var query = new GetModuleListQuery() { PathId = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldReturnNotFound_WhenModuleIdNotFound()
    {
      var query = new GetModuleQuery() { PathId = 1, Id = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldReturnNotFound_WhenPathIdNotFound()
    {
      var query = new GetModuleQuery() { PathId = 99999, Id = 1 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void DetailsShouldReturnNotFound_WhenModuleIdNotFound()
    {
      var query = new GetModuleDetailsQuery() { PathId = 1, Id = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void DetailsReturnNotFound_WhenPathIdNotFound()
    {
      var query = new GetModuleDetailsQuery() { PathId = 99999, Id = 1 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }
  }
}
