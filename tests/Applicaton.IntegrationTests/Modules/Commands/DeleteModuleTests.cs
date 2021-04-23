using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class DeleteModuleTests : TestBase
  {
    [Test]
    public void ShouldRequireValidPathId()
    {
      var command = new DeleteModuleCommand { PathId = 999999, Id = 1 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireValidModuleId()
    {
      var command = new DeleteModuleCommand { PathId = 1, Id = 999999 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldRemoveModuleFromPath()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        Paths = new List<Path> { new Path
          {
            Title = "Some Path1",
            Description = "Some Path Description"
          }, 
          new Path
          {
            Title = "Some Path2",
            Description = "Some Path Description"
          } 
        }
      });

      var pathId = module.Paths.FirstOrDefault().Id;
      var query = new GetModuleQuery() { PathId = pathId, Id = module.Id };
      var moduleAdded = await SendAsync(query);

      await SendAsync(new DeleteModuleCommand
      {
        PathId = module.Paths.FirstOrDefault().Id,
        Id = module.Id
      });

      pathId = module.Paths.FirstOrDefault().Id;
      query = new GetModuleQuery() { PathId = pathId, Id = module.Id };
      var moduleRemovedFromPath1 = await SendAsync(query);

      moduleAdded.Should().NotBeNull();
      moduleAdded.Paths.Should().HaveCount(2);
      moduleRemovedFromPath1.Should().NotBeNull();
      moduleRemovedFromPath1.Paths.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldDeleteModule()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        Paths = new List<Path> { new Path
        {
          Title = "Some Path1",
          Description = "Some Path Description"
        }}
      });

      var moduleAdded = await FindAsync<Module>(module.Id);

      await SendAsync(new DeleteModuleCommand
      {
        PathId = module.Paths.FirstOrDefault().Id,
        Id = module.Id
      });

      var moduleDeleted = await FindAsync<Module>(module.Id);

      moduleAdded.Should().NotBeNull();
      moduleDeleted.Should().BeNull();
    }
  }
}