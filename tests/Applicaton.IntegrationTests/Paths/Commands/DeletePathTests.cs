using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.DeletePath;
using DeveloperPath.Domain.Entities;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Paths.Commands;

using static Testing;

public class DeletePathTests : TestBase
{
  [Test]
  public void ShouldRequireValidPathId()
  {
    var command = new DeletePath { Id = 99 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public async Task ShouldDeletePath()
  {
    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    var pathAdded = await FindAsync<Path>(path.Id);

    await SendAsync(new DeletePath
    {
      Id = path.Id
    });

    var pathDeleted = await FindAsync<Path>(path.Id);

    Assert.That(pathAdded, Is.Not.Null);
    Assert.That(pathDeleted, Is.Null);
  }
}