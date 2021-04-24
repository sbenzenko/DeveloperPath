using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.DeletePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class DeletePathTests : TestBase
  {
    [Test]
    public void ShouldRequireValidPathId()
    {
      var command = new DeletePath { Id = 99 };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeletePath()
    {
      var path = await SendAsync(new CreatePath
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var pathAdded = await FindAsync<Path>(path.Id);

      await SendAsync(new DeletePath
      {
        Id = path.Id
      });

      var pathDeleted = await FindAsync<Path>(path.Id);

      pathAdded.Should().NotBeNull();
      pathDeleted.Should().BeNull();
    }
  }
}
