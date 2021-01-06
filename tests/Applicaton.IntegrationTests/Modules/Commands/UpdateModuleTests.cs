using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Application.Paths.Commands.UpdatePath;
using DeveloperPath.Application.TodoLists.Commands.CreateTodoList;
using DeveloperPath.Application.TodoLists.Commands.UpdateTodoList;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Commands
{
  using static Testing;

  public class UpdateModuleTests : TestBase
  {
    //[Test]
    //public void ShouldRequireValidPathId()
    //{
    //  var command = new UpdatePathCommand
    //  {
    //    Id = 99,
    //    Title = "New Title",
    //    Description = "New Description"
    //  };

    //  FluentActions.Invoking(() =>
    //      SendAsync(command)).Should().Throw<NotFoundException>();
    //}

    //[Test]
    //public async Task ShouldRequireUniqueTitle()
    //{
    //  var pathId = await SendAsync(new CreatePathCommand
    //  {
    //    Title = "New Path",
    //    Description = "New Path Description"
    //  });

    //  await SendAsync(new CreatePathCommand
    //  {
    //    Title = "Other New Path",
    //    Description = "New Other Path Description"
    //  });

    //  var command = new UpdatePathCommand
    //  {
    //    Id = pathId,
    //    Title = "Other New Path",
    //    Description = "New Path Description"
    //  };

    //  FluentActions.Invoking(() =>
    //      SendAsync(command))
    //          .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
    //          .And.Errors["Title"].Should().Contain("The specified title already exists.");
    //}

    //[Test]
    //public async Task ShouldUpdatePath()
    //{
    //  var userId = await RunAsDefaultUserAsync();

    //  var pathId = await SendAsync(new CreatePathCommand
    //  {
    //    Title = "New Path",
    //    Description = "New Path Description"
    //  });

    //  var command = new UpdatePathCommand
    //  {
    //    Id = pathId,
    //    Title = "Updated Path Title",
    //    Description = "New Path Description"
    //  };

    //  await SendAsync(command);

    //  var path = await FindAsync<Path>(pathId);

    //  path.Title.Should().Be(command.Title);
    //  path.LastModifiedBy.Should().NotBeNull();
    //  path.LastModifiedBy.Should().Be(userId);
    //  path.LastModified.Should().NotBeNull();
    //  path.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    //}
  }
}
