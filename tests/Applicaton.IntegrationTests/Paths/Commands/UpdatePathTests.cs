using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Domain.Entities;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Paths.Commands;

using static Testing;

public class UpdatePathTests : TestBase
{
  [Test]
  public void ShouldRequireValidPathId()
  {
    var command = new UpdatePath
    {
      Id = 99,
      Title = "New Title",
      Key = "some-path",
      Description = "New Description"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }


  [Test]
  public async Task ShouldRequireTitle()
  {
    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    var command = new UpdatePath
    {
      Id = path.Id,
      Title = "",
      Description = "New Path Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public async Task ShouldDisallowLongTitle()
  {
    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    var command = new UpdatePath
    {
      Id = path.Id,
      Title = "This path title is too long and exceeds one hundred characters allowed for path titles by UpdatePathCommandValidator",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 100 characters."), Is.True);
  }

  [Test]
  public async Task ShouldRequireUniqueTitle()
  {
    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    await SendAsync(new CreatePath
    {
      Title = "Other New Path",
      Key = "some-path-other",
      Description = "New Other Path Description"
    });

    var command = new UpdatePath
    {
      Id = path.Id,
      Title = "Other New Path",
      Description = "New Path Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("The specified path already exists."), Is.True);
  }

  [Test]
  public async Task ShouldUpdatePath()
  {
    var userId = await RunAsDefaultUserAsync();

    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    var command = new UpdatePath
    {
      Id = path.Id,
      Key = "some-path",
      Title = "Updated Path Title",
      Description = "New Path Description"
    };

    await SendAsync(command);

    var updatedPath = await FindAsync<Path>(path.Id);

    Assert.That(updatedPath.Title, Is.EqualTo(command.Title));
    Assert.That(updatedPath.LastModifiedBy, Is.Not.Null);
    Assert.That(updatedPath.LastModifiedBy, Is.EqualTo(userId));
    Assert.That(updatedPath.LastModified, Is.Not.Null);
    Assert.That(updatedPath.LastModified, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }
}