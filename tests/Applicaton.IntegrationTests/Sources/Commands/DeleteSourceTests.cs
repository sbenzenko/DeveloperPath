using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Sources.Commands.DeleteSource;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class DeleteSourceTests : TestBase
  {
    [Test]
    public void ShouldRequireValidSourceId()
    {
      var command = new DeleteSourceCommand { PathId = 1, ModuleId = 9999, ThemeId = 1, Id = 99999 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteSource()
    {
      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var source = await AddAsync(new Source
      {
        ThemeId = theme.Id,
        Title = "Source 1",
        Description = "Some description",
        Url = "https://source1.com",
        Order = 0,
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var sourceAdded = await FindAsync<Source>(source.Id);

      await SendAsync(new DeleteSourceCommand
      {
        PathId = path.Id,
        ModuleId = module.Id,
        ThemeId = theme.Id,
        Id = source.Id
      });

      var sourceDeleted = await FindAsync<Source>(source.Id);

      sourceAdded.Should().NotBeNull();
      sourceDeleted.Should().BeNull();
    }
  }
}
