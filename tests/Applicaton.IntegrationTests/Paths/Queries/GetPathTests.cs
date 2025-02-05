using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Paths.Queries;

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

    Assert.That(result, Has.Count.EqualTo(4));
  }

  //[Test]
  //public async Task GetPaged_ShouldReturnPathList()
  //{
  //  await AddAsync(new Path { Title = "Path5", Key = "some-path5", Description = "Description 1" });
  //  await AddAsync(new Path { Title = "Path6", Key = "some-path6", Description = "Description 2" });
  //  await AddAsync(new Path { Title = "Path7", Key = "some-path7", Description = "Description 3" });
  //  await AddAsync(new Path { Title = "Path8", Key = "some-path8", Description = "Description 4" });

  //  var query = new GetPathListQueryPaging { PageNumber = 2, PageSize = 1 };

  //  var result = await SendAsync(query);

  //  result.Item1.PageSize.Should().Be(1);
  //  result.Item1.PageNumber.Should().Be(2);
  //  result.Item2.Should().HaveCount(1);
  //  (result.Item2.ToList())[0].Title.Should().Be("Path6");
  //}

  //[Test]
  //public async Task GetPagedOutOfRange_ShouldReturnEmpty()
  //{
  //  var query = new GetPathListQueryPaging { PageSize = 999, PageNumber = 3 };

  //  var result = await SendAsync(query);

  //  result.Item1.PageSize.Should().Be(999);
  //  result.Item1.PageNumber.Should().Be(3);
  //  result.Item2.Should().BeEmpty();
  //}

  [Test]
  public void Get_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetPathListQuery();

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public void GetPaged_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetPathPagedListQuery { PageNumber = 1, PageSize = 1 };

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public async Task GetOne_ShouldReturnPath()
  {
    var path = await AddAsync(new Path
    {
      Title = "Some Path",
      Key = "some-path",
      Description = "Some Path Description",
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var query = new GetPathQuery() { Id = path.Id };

    var result = await SendAsync(query);

    Assert.That(result.Title, Is.Not.Empty);
    Assert.That(result.Tags, Has.Count.EqualTo(3));
  }

  [Test]
  public void GetOne_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetPathQuery() { Id = 1 };

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public void ShouldReturnNotFound_WhenIdNotFound()
  {
    var query = new GetPathQuery() { Id = 99999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public async Task ShouldReturnPathWithModules()
  {
    var path = await AddAsync(new Path
    {
      Title = "Some Other Path",
      Key = "some-path",
      Description = "Some Other Path Description",
      Modules =
        [
          new() { Title = "Module1", Key = "module-key-1",Description = "Module 1 Description", Necessity = Necessity.Other },
          new() { Title = "Module2", Key = "module-key-2",Description = "Module 2 Description", Necessity = Necessity.GoodToKnow },
          new() { Title = "Module3", Key = "module-key-3",Description = "Module 3 Description", Necessity = Necessity.Interesting },
          new() { Title = "Module4", Key = "module-key-4",Description = "Module 4 Description", Necessity = Necessity.MustKnow },
          new() { Title = "Module5", Key = "module-key-5",Description = "Module 5 Description", Necessity = Necessity.Possibility }
        ],
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var query = new GetPathDetailsQuery() { Id = path.Id };

    var result = await SendAsync(query);

    Assert.That(result.Title, Is.Not.Empty);
    Assert.That(result.Modules, Has.Count.EqualTo(5));
    Assert.That(result.Tags, Has.Count.EqualTo(3));
  }

  [Test]
  public void DetailsShouldReturnNotFound_WhenIdNotFound()
  {
    var query = new GetPathDetailsQuery() { Id = 99999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }
}