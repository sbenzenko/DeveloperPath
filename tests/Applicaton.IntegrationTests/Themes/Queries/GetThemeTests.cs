using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Themes.Queries;

using static Testing;

public class GetThemeTests : TestBase
{
  [Test]
  public async Task Get_ShouldReturnThemesList()
  {
    var path = await AddAsync(
      new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Other Module",
      Description = "New Other Module Description",
      Necessity = Necessity.MustKnow,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });
    await AddAsync(new Theme
    {
      Title = "New Theme1",
      ModuleId = module.Id,
      Description = "New Theme1 Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Tags = ["Theme1", "ThemeTag2", "Tag3"],
      Order = 1
    });
    await AddAsync(new Theme
    {
      Title = "New Theme2",
      ModuleId = module.Id,
      Description = "New Theme2 Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Tags = ["Theme2", "ThemeTag2", "Tag3"],
      Order = 2
    });
    await AddAsync(new Theme
    {
      Title = "New Theme3",
      ModuleId = module.Id,
      Description = "New Theme3 Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Tags = ["Theme2", "ThemeTag2", "Tag3"],
      Order = 3
    });

    var query = new GetThemeListQuery { PathId = path.Id, ModuleId = module.Id };

    var result = await SendAsync(query);

    Assert.That(result, Has.Count.EqualTo(3));
    Assert.That(result.ToList()[2].Title, Is.EqualTo("New Theme3"));
  }

  [Test]
  public void Get_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetThemeListQuery() { PathId = 1, ModuleId = 1 };

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public async Task GetOne_ShouldReturnTheme()
  {
    var path = await AddAsync(
      new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module Module",
      Description = "New Module Description",
      Necessity = Necessity.MustKnow,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      ModuleId = module.Id,
      Description = "New Theme Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Tags = ["Theme1", "ThemeTag2", "Tag3"],
      Order = 2,
      Section = new Section
      {
        ModuleId = module.Id,
        Title = "First Section"
      }
    });

    var query = new GetThemeQuery() { PathId = path.Id, ModuleId = module.Id, Id = theme.Id };

    var createdTheme = await SendAsync(query);

    Assert.That(createdTheme, Is.Not.Null);
    Assert.That(createdTheme.Title, Is.Not.Empty);
    Assert.That(createdTheme.Description, Is.Not.Empty);
    Assert.That(createdTheme.Section, Is.Not.Null);
    Assert.That(createdTheme.ModuleId, Is.EqualTo(module.Id));
    Assert.That(createdTheme.Tags, Has.Count.EqualTo(3));
  }

  [Test]
  public void GetOne_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetThemeQuery() { PathId = 1, ModuleId = 1, Id = 1 };

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public async Task GetDetails_ShouldReturnThemeDetails()
  {
    var path = await AddAsync(
      new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module Module",
      Description = "New Module Description",
      Necessity = Necessity.MustKnow,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Other Theme",
      ModuleId = module.Id,
      Description = "New Other Theme Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Tags = ["Theme1", "ThemeTag2", "Tag3"],
      Order = 2,
      Sources =
      [
        new Source {
            Title = "Source1",
            Type =  SourceType.Blog,
            Url = "https://www.google.com",
            Availability = Availability.Free },
        new Source {
            Title = "Source2",
            Type = SourceType.Blog,
            Url = "https://www.microsoft.com",
            Availability = Availability.RequiresRegistration }
      ],
      Section = new Section
      {
        ModuleId = module.Id,
        Title = "First Section"
      }
    });

    var query = new GetThemeDetailsQuery() { Id = theme.Id, PathId = path.Id, ModuleId = module.Id };

    var createdTheme = await SendAsync(query);

    Assert.That(createdTheme, Is.Not.Null);
    Assert.That(createdTheme.Id, Is.EqualTo(theme.Id));
    Assert.That(createdTheme.Title, Is.Not.Empty);
    Assert.That(createdTheme.Description, Is.Not.Empty);
    Assert.That(createdTheme.Section, Is.Not.Null);
    Assert.That(createdTheme.Module, Is.Not.Null);
    Assert.That(createdTheme.Module.Id, Is.EqualTo(module.Id));
    Assert.That(createdTheme.Sources, Has.Count.EqualTo(2));
    Assert.That(createdTheme.Tags, Has.Count.EqualTo(3));
  }

  [Test]
  public void GetDetails_ShouldThrow_WhenCanceled()
  {
    var cts = new CancellationTokenSource();
    cts.Cancel();

    var query = new GetThemeDetailsQuery() { PathId = 1, ModuleId = 1, Id = 1 };

    Assert.ThrowsAsync<TaskCanceledException>(() => SendAsync(query, cts.Token));
  }

  [Test]
  public void ListShouldReturnNotFound_WhenPathIdNotFound()
  {
    var query = new GetThemeListQuery() { PathId = 99999, ModuleId = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void ListShouldReturnNotFound_WhenModuleIdNotFound()
  {
    var query = new GetThemeListQuery() { PathId = 1, ModuleId = 99999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void ShouldReturnNotFound_WhenIdNotFound()
  {
    var query = new GetThemeQuery() { PathId = 1, ModuleId = 1, Id = 99999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void ShouldReturnNotFound_WhenModuleIdNotFound()
  {
    var query = new GetThemeQuery() { PathId = 1, ModuleId = 99999, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void ShouldReturnNotFound_WhenPathIdNotFound()
  {
    var query = new GetThemeQuery() { PathId = 99999, ModuleId = 1, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void DetailsShouldReturnNotFound_WhenIdNotFound()
  {
    var query = new GetThemeDetailsQuery() { PathId = 1, ModuleId = 1, Id = 99999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void DetailsShouldReturnNotFound_WhenModuleIdNotFound()
  {
    var query = new GetThemeDetailsQuery() { PathId = 1, ModuleId = 99999, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }

  [Test]
  public void DetailsShouldReturnNotFound_WhenPathIdNotFound()
  {
    var query = new GetThemeDetailsQuery() { PathId = 99999, ModuleId = 1, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
  }
}