using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Domain.Entities;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Modules.Commands;

using static Testing;

public class UpdateModuleTests : TestBase
{
  [Test]
  public void ShouldRequireValidModuleId()
  {
    var command = new UpdateModule
    {
      Id = 99,
      Title = "New Title",
      Key = "new-module",
      Description = "New Description",
      Necessity = 0
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

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module",
      Description = "New Module Description"
    });

    var command = new UpdateModule
    {
      Id = module.Id,
      Title = "",
      Description = "New Description",
      Necessity = 0
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public async Task ShouldRequireDescription()
  {
    var path = await SendAsync(new CreatePath
    {
      Title = "New Path",
      Key = "some-path",
      Description = "New Path Description"
    });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module",
      Description = "New Module Description"
    });

    var command = new UpdateModule
    {
      Id = module.Id,
      Title = "Updated Module",
      Description = "",
      Necessity = 0
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Description"), Is.True);
    Assert.That(ex.Errors["Description"].Contains("Description is required."), Is.True);
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

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module",
      Description = "New Module Description"
    });

    var command = new UpdateModule
    {
      Id = module.Id,
      Title = "This module title is too long and exceeds one hundred characters allowed for module titles by UpdateModuleCommandValidator",
      Description = "New Description",
      Necessity = 0
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 100 characters."), Is.True);
  }

  //TODO: re-work logic of unique module in the Path 
  //[Test]
  //public async Task ShouldRequireUniqueTitle()
  //{
  //    var path = await AddAsync(new Path
  //    {
  //        Title = "New Path",
  //        Key = "some-path",
  //        Description = "New Path Description"
  //    });

  //    var module = await SendAsync(new CreateModule
  //    {
  //        Key = "module-key",
  //        Title = "New Module",
  //        Description = "New Module Description"
  //    });

  //    await SendAsync(new CreateModule
  //    {
  //        Key = "module-key-two",
  //        Title = "Other New Module",
  //        Description = "New Other Path Description"
  //    });

  //    var command = new UpdateModule
  //    {
  //        Id = module.Id,
  //        Title = "Other New Module",
  //        Description = "New Module Description"
  //    };

  //    FluentActions.Invoking(() =>
  //        SendAsync(command))
  //            .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
  //            .Result.And.Errors["Title"].Should().Contain("A module with this title already exists in one of associated paths.");
  //}

  [Test]
  public async Task ShouldUpdateModule()
  {
    var userId = await RunAsDefaultUserAsync();

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "New Module",
      Description = "New Module Description"
    });

    var command = new UpdateModule
    {
      Id = module.Id,
      Title = "Updated title",
      Key = "module-key",
      Description = "Updated Description",
      Necessity = 0
    };

    await SendAsync(command);

    var updatedModule = await FindAsync<Module>(module.Id);

    Assert.That(updatedModule.Title, Is.EqualTo(command.Title));
    Assert.That(updatedModule.Description, Is.EqualTo(command.Description));
    Assert.That(updatedModule.Key, Is.EqualTo(module.Key));
    Assert.That(updatedModule.LastModifiedBy, Is.Not.Null);
    Assert.That(updatedModule.LastModifiedBy, Is.EqualTo(userId));
    Assert.That(updatedModule.LastModified, Is.Not.Null);
    Assert.That(updatedModule.LastModified, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }
}
