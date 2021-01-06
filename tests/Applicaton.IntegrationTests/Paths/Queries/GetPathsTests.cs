using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Paths.Queries.GetPaths;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Queries
{
  using static Testing;

  public class GetPathsTests : TestBase
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
    public async Task ShouldReturnPathWithModules()
    {
      var pathId = await AddWithIdAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description",
        Modules = new List<Module>
          {
            new Module { Title = "Module1", Description = "Module 1 Description", Necessity = NecessityLevel.Other },
            new Module { Title = "Module2", Description = "Module 2 Description", Necessity = NecessityLevel.GoodToKnow },
            new Module { Title = "Module3", Description = "Module 3 Description", Necessity = NecessityLevel.Interesting },
            new Module { Title = "Module4", Description = "Module 4 Description", Necessity = NecessityLevel.MustKnow },
            new Module { Title = "Module5", Description = "Module 5 Description", Necessity = NecessityLevel.Possibility }
          },
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var query = new GetPathQuery() { Id = pathId };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Modules.Should().HaveCount(5);
      result.Tags.Should().HaveCount(3);
    }
  }
}
