using System;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.UpdatePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Commands
{
  using static Testing;

  public class UpdatePathTests : TestBase
  {
    [Test]
    public void ShouldRequireValidPathId()
    {
      var command = new UpdatePathCommand
      {
        Id = 99,
        Title = "New Title",
        Description = "New Description"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      var path = await SendAsync(new CreatePathCommand
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      await SendAsync(new CreatePathCommand
      {
        Title = "Other New Path",
        Description = "New Other Path Description"
      });

      var command = new UpdatePathCommand
      {
        Id = path.Id,
        Title = "Other New Path",
        Description = "New Path Description"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("The specified path already exists.");
    }

    [Test]
    public async Task ShouldUpdatePath()
    {
      var userId = await RunAsDefaultUserAsync();

      var path = await SendAsync(new CreatePathCommand
      {
        Title = "New Path",
        Description = "New Path Description"
      });

      var command = new UpdatePathCommand
      {
        Id = path.Id,
        Title = "Updated Path Title",
        Description = "New Path Description"
      };

      await SendAsync(command);

      var updatedPath = await FindAsync<Path>(path.Id);

      updatedPath.Title.Should().Be(command.Title);
      updatedPath.LastModifiedBy.Should().NotBeNull();
      updatedPath.LastModifiedBy.Should().Be(userId);
      updatedPath.LastModified.Should().NotBeNull();
      updatedPath.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    }
  }
}
