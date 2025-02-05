using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.CreateSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Sources.Commands;

using static Testing;

public class CreateSourceTests : TestBase
{
  [Test]
  public void ShouldRequireMinimumFields()
  {
    var command = new CreateSource();

    Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireModuleId()
  {
    var command = new CreateSource
    {
      ThemeId = 1,
      Title = "Source Title",
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("ModuleId"), Is.True);
    Assert.That(ex.Errors["ModuleId"].Contains("Module Id is required."), Is.True);
  }

  [Test]
  public void ShouldRequireThemeId()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      Title = "Source Title",
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("ThemeId"), Is.True);
    Assert.That(ex.Errors["ThemeId"].Contains("Theme Id is required."), Is.True);
  }

  [Test]
  public void ShouldReturnNotFoundForNonExistingModule()
  {
    var command = new CreateSource
    {
      ModuleId = 999,
      ThemeId = 1,
      Title = "Source Title",
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldReturnNotFoundForNonExistingTheme()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      ThemeId = 999,
      Title = "Source Title",
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireTitle()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      ThemeId = 1,
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public void ShouldDisallowLongTitle()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      ThemeId = 1,
      Title = "This source title is too long and exceeds two hundred characters allowed for theme titles by CreateSourceCommandValidator. And this source title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
      Description = "Source Description",
      Url = "http://www.ww.ww"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 200 characters."), Is.True);
  }

  [Test]
  public void ShouldRequireUrl()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      ThemeId = 1,
      Title = "Source Title",
      Description = "Source Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Url"), Is.True);
    Assert.That(ex.Errors["Url"].Contains("URL is required."), Is.True);
  }

  [Test]
  public void ShouldCheckUrlFormat()
  {
    var command = new CreateSource
    {
      ModuleId = 1,
      ThemeId = 1,
      Title = "Source Title",
      Description = "Source Description",
      Url = "http:someinvalidurl"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Url"), Is.True);
    Assert.That(ex.Errors["Url"].Contains("URL must be in valid format, e.g. http://www.domain.com."), Is.True);
  }

  [Test]
  public async Task ShouldCreateSource()
  {
    //var userId = await RunAsDefaultUserAsync();

    var path = await AddAsync(new Path
    {
      Title = "Some Path",
      Key = "some-path",
      Description = "Some Path Description"
    });

    var module = await SendAsync(new CreateModule
    {
      Key = "module-key",
      Title = "Module Title",
      Description = "Module Description"
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      ModuleId = module.Id,
      Description = "New Theme Description",
      Necessity = Necessity.MustKnow,
      Complexity = Complexity.Beginner,
      Order = 2
    });

    var command = new CreateSource
    {
      PathId = path.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Title = "New Theme",
      Description = "New Theme Description",
      Url = "https://www.test.com",
      Type = SourceType.Book,
      Availability = Availability.RequiresRegistration,
      Relevance = Relevance.Relevant,
      Order = 1
    };

    var createdSource = await SendAsync(command);

    var source = await FindAsync<Source>(createdSource.Id);

    Assert.That(source, Is.Not.Null);
    Assert.That(source.ThemeId, Is.EqualTo(command.ThemeId));
    Assert.That(source.Title, Is.EqualTo(command.Title));
    Assert.That(source.Description, Is.EqualTo(command.Description));
    Assert.That(source.Url, Is.EqualTo(command.Url));
    Assert.That(source.Order, Is.EqualTo(command.Order));
    Assert.That(source.Type, Is.EqualTo(command.Type));
    Assert.That(source.Availability, Is.EqualTo(command.Availability));
    Assert.That(source.Relevance, Is.EqualTo(command.Relevance));
    // Assert.That(source.CreatedBy, Is.EqualTo(userId));
  }
}