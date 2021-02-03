using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Modules.Commands.DeleteModule;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class DeleteModuleTests : TestBase
  {
    [Test]
    public void ShouldRequireValidModuleId()
    {
      var command = new DeleteModuleCommand { Id = 99 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteModule()
    {
      var moduleId = await AddWithIdAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var moduleAdded = await FindAsync<Module>(moduleId);

      await SendAsync(new DeleteModuleCommand
      {
        Id = moduleId
      });

      var moduleDeleted = await FindAsync<Module>(moduleId);

      moduleAdded.Should().NotBeNull();
      moduleDeleted.Should().BeNull();
    }
  }
}
