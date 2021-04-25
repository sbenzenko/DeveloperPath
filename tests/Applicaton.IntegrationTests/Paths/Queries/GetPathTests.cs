using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Queries
{
  using static Testing;

  public class GetPathTests : TestBase
  {
    [Test]
    public async Task ShouldReturnPathList()
    {
      await AddAsync(new Path { Title = "Path1", Description = "Description 1" });
      await AddAsync(new Path { Title = "Path2", Description = "Description 2" });
      await AddAsync(new Path { Title = "Path3", Description = "Description 3" });
      await AddAsync(new Path { Title = "Path4", Description = "Description 4" });

      var query = new GetPathListQuery();

      var result = await SendAsync(query);

      result.Should().HaveCount(4);
    }

    [Test]
    public async Task ShouldReturnPath()
    {
      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var query = new GetPathQuery() { Id = path.Id };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Tags.Should().HaveCount(3);
    }

    [Test]
    public void ShouldReturnNotFound_WhenIdNotFound()
    {
      var query = new GetPathQuery() { Id = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldReturnPathWithModules()
    {
      var path = await AddAsync(new Path
      {
        Title = "Some Other Path",
        Description = "Some Other Path Description",
        Modules = new List<Module>
          {
            new Module { Title = "Module1", Description = "Module 1 Description", Necessity = Necessity.Other },
            new Module { Title = "Module2", Description = "Module 2 Description", Necessity = Necessity.GoodToKnow },
            new Module { Title = "Module3", Description = "Module 3 Description", Necessity = Necessity.Interesting },
            new Module { Title = "Module4", Description = "Module 4 Description", Necessity = Necessity.MustKnow },
            new Module { Title = "Module5", Description = "Module 5 Description", Necessity = Necessity.Possibility }
          },
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var query = new GetPathDetailsQuery() { Id = path.Id };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Modules.Should().HaveCount(5);
      result.Tags.Should().HaveCount(3);
    }

    [Test]
    public void DetailsShouldReturnNotFound_WhenIdNotFound()
    {
      var query = new GetPathDetailsQuery() { Id = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(query)).Should().Throw<NotFoundException>();
    }
  }
}
