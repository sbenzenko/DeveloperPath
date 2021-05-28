using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public async Task Get_ShouldReturnPathList()
    {
      await AddAsync(new Path { Title = "Path1", Key = "some-path1", Description = "Description 1" });
      await AddAsync(new Path { Title = "Path2", Key = "some-path2", Description = "Description 2" });
      await AddAsync(new Path { Title = "Path3", Key = "some-path3", Description = "Description 3" });
      await AddAsync(new Path { Title = "Path4", Key = "some-path4", Description = "Description 4" });

      var query = new GetPathListQuery();

      var result = await SendAsync(query);

      result.Should().HaveCount(4);
    }

    [Test]
    public async Task GetPaged_ShouldReturnPathList()
    {
      await AddAsync(new Path { Title = "Path5", Key = "some-path5", Description = "Description 1" });
      await AddAsync(new Path { Title = "Path6", Key = "some-path6", Description = "Description 2" });
      await AddAsync(new Path { Title = "Path7", Key = "some-path7", Description = "Description 3" });
      await AddAsync(new Path { Title = "Path8", Key = "some-path8", Description = "Description 4" });

      var query = new GetPathListQueryPaging { PageNumber = 2, PageSize = 1 };

      var result = await SendAsync(query);

      result.Item1.PageSize.Should().Be(1);
      result.Item1.PageNumber.Should().Be(2);
      result.Item2.Should().HaveCount(1);
      (result.Item2.ToList())[0].Title.Should().Be("Path6");
    }

    [Test]
    public async Task GetPagedOutOfRange_ShouldReturnEmpty()
    {
      var query = new GetPathListQueryPaging { PageSize = 999, PageNumber = 3 };

      var result = await SendAsync(query);

      result.Item1.PageSize.Should().Be(999);
      result.Item1.PageNumber.Should().Be(3);
      result.Item2.Should().BeEmpty();
    }

    [Test]
    public void Get_ShouldThrow_WhenCanceled()
    {
      var cts = new CancellationTokenSource();
      cts.Cancel();

      var query = new GetPathListQuery();

      FluentActions.Invoking(() =>
          SendAsync(query, cts.Token)).Should().Throw<TaskCanceledException>();
    }

    [Test]
    public void GetPaged_ShouldThrow_WhenCanceled()
    {
      var cts = new CancellationTokenSource();
      cts.Cancel();

      var query = new GetPathListQueryPaging { PageNumber = 1, PageSize = 1 };

      FluentActions.Invoking(() =>
          SendAsync(query, cts.Token)).Should().Throw<TaskCanceledException>();
    }

    [Test]
    public async Task GetOne_ShouldReturnPath()
    {
      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Key = "some-path",
        Description = "Some Path Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var query = new GetPathQuery() { Id = path.Id };

      var result = await SendAsync(query);

      result.Title.Should().NotBeEmpty();
      result.Tags.Should().HaveCount(3);
    }

    [Test]
    public void GetOne_ShouldThrow_WhenCanceled()
    {
      var cts = new CancellationTokenSource();
      cts.Cancel();

      var query = new GetPathQuery() { Id = 1 };

      FluentActions.Invoking(() =>
          SendAsync(query, cts.Token)).Should().Throw<TaskCanceledException>();
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
        Key = "some-path",
        Description = "Some Other Path Description",
        Modules = new List<Module>
          {
            new Module { Title = "Module1", Key = "module-key-1",Description = "Module 1 Description", Necessity = Necessity.Other },
            new Module { Title = "Module2", Key = "module-key-2",Description = "Module 2 Description", Necessity = Necessity.GoodToKnow },
            new Module { Title = "Module3", Key = "module-key-3",Description = "Module 3 Description", Necessity = Necessity.Interesting },
            new Module { Title = "Module4", Key = "module-key-4",Description = "Module 4 Description", Necessity = Necessity.MustKnow },
            new Module { Title = "Module5", Key = "module-key-5",Description = "Module 5 Description", Necessity = Necessity.Possibility }
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
