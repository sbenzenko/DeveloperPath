using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Sources.Commands;

using static Testing;

public class DeleteSourceTests : TestBase
{
  [Test]
  public void ShouldRequireValidPathId()
  {
    var command = new DeleteSource { PathId = 999999, ModuleId = 1, ThemeId = 1, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }
  [Test]
  public void ShouldRequireValidModuleId()
  {
    var command = new DeleteSource { PathId = 1, ModuleId = 999999, ThemeId = 1, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }
  [Test]
  public void ShouldRequireValidThemeId()
  {
    var command = new DeleteSource { PathId = 1, ModuleId = 1, ThemeId = 999999, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireValidSourceId()
  {
    var command = new DeleteSource { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 999999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public async Task ShouldDeleteSource()
  {
    var path = await AddAsync(new Path
    {
      Title = "Some Path",
      Key = "some-path",
      Description = "Some Path Description"
    });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "Module Title",
      Description = "Module Description"
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      ModuleId = module.Id
    });

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var sourceAdded = await FindAsync<Source>(source.Id);

    await SendAsync(new DeleteSource
    {
      PathId = path.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Id = source.Id
    });

    var sourceDeleted = await FindAsync<Source>(source.Id);

    Assert.That(sourceAdded, Is.Not.Null);
    Assert.That(sourceDeleted, Is.Null);
  }
}