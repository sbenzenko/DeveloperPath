using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Modules.Commands;

using static Testing;

public class CreateModuleTests : TestBase
{
  [Test]
  public void ShouldRequireMinimumFields()
  {
    var command = new CreateModule();

    Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
  }

  // TODO: Verify if this test is needed
  //[Test]
  //public void ShouldReturnNotFoundForNonExistingPath()
  //{
  //  var command = new CreateModule
  //  {
  //    Title = "New Title",
  //    Key = "module-key",
  //    Description = "New Description",
  //    Necessity = 0
  //  };

  //  Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  //}

  [Test]
  public void ShouldRequireTitle()
  {
    var command = new CreateModule
    {
      Title = "",
      Description = "Module Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public void ShouldDisallowLongTitle()
  {
    var command = new CreateModule
    {
      Title = "This module title is too long and exceeds one hundred characters allowed for module titles by CreateModuleCommandValidator",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 100 characters."), Is.True);
  }

  [Test]
  public void ShouldRequireDescription()
  {
    var command = new CreateModule
    {
      Title = "Module Title"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Description"), Is.True);
    Assert.That(ex.Errors["Description"].Contains("Description is required."), Is.True);
  }

  //[Test]
  //public async Task ShouldRequireUniqueTitle()
  //{
  //    var path = await AddAsync(new Path
  //    {
  //        Title = "Some Path",
  //        Key = "some-path",
  //        Description = "Some Path Description"
  //    });

  //    await SendAsync(new CreateModule
  //    {
  //        Key = "module-key",
  //        Title = "Module Title",
  //        Description = "Module Description"
  //    });

  //    var command = new CreateModule
  //    {
  //        Key = "module-key-two",
  //        Title = "Module Title",
  //        Description = "Module Description"
  //    };

  //    FluentActions.Invoking(() =>
  //        SendAsync(command)).Should().ThrowAsync<ValidationException>()
  //          .Where(ex => ex.Errors.ContainsKey("Title"))
  //          .Result.And.Errors["Title"].Should().Contain("The specified module already exists in this path.");
  //}

  [Test]
  public async Task ShouldCreateModule()
  {
    var userId = await RunAsDefaultUserAsync();

    var command = new CreateModule
    {
      Title = "New Module",
      Key = "new-module",
      Description = "New Module Description",
      Necessity = Necessity.Other,
      Tags = ["Tag1", "Tag2", "Tag3"]
    };

    var createdModule = await SendAsync(command);

    var module = await FindAsync<Module>(createdModule.Id);

    Assert.That(module, Is.Not.Null);
    Assert.That(module.Title, Is.EqualTo(command.Title));
    Assert.That(module.Key, Is.EqualTo(command.Key));
    Assert.That(module.Description, Is.EqualTo(command.Description));
    Assert.That(module.Necessity, Is.EqualTo(command.Necessity));
    Assert.That(module.CreatedBy, Is.EqualTo(userId));
  }
}