using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Themes.Commands.CreateTheme;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Themes.Commands;

using static Testing;

public class CreateThemeTests : TestBase
{
  [Test]
  public void ShouldRequireMinimumFields()
  {
    var command = new CreateTheme();

    Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireModuleId()
  {
    var command = new CreateTheme
    {
      PathId = 1,
      Title = "Theme Title",
      Description = "Theme Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("ModuleId"), Is.True);
    Assert.That(ex.Errors["ModuleId"].Contains("Module Id is required."), Is.True);
  }

  [Test]
  public void ShouldReturnNotFoundForNonExistingModule()
  {
    var command = new CreateTheme
    {
      ModuleId = 999,
      Title = "Theme Title",
      Description = "Theme Description"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireTitle()
  {
    var command = new CreateTheme
    {
      ModuleId = 1,
      Title = "",
      Description = "Theme Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public void ShouldDisallowLongTitle()
  {
    var command = new CreateTheme
    {
      ModuleId = 1,
      Title = "This theme title is too long and exceeds two hundred characters allowed for theme titles by CreateThemeCommandValidator. And this theme title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
      Description = "Theme Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 200 characters."), Is.True);
  }

  [Test]
  public void ShouldRequireDescription()
  {
    var command = new CreateTheme
    {
      ModuleId = 1,
      Title = "Theme Title"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Description"), Is.True);
    Assert.That(ex.Errors["Description"].Contains("Description is required."), Is.True);
  }

  [Test]
  public async Task ShouldRequireUniqueTitle()
  {
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

    await SendAsync(new CreateTheme
    {
      PathId = path.Id,
      ModuleId = module.Id,
      Title = "Theme Title",
      Description = "Theme Description"
    });

    var command = new CreateTheme
    {
      ModuleId = module.Id,
      Title = "Theme Title",
      Description = "Theme Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("The specified theme already exists in this module."), Is.True);
  }

  [Test]
  public async Task ShouldCreateThemeWithoutSection()
  {
    var userId = await RunAsDefaultUserAsync();

    var path = await AddAsync(new Path
    {
      Title = "Some Path",
      Key = "some-path",
      Description = "Some Path Description"
    });

    var module = await SendAsync(new CreateModule
    {
      Title = "Module Title",
      Key = "module-key",
      Description = "Module Description"
    });

    var command = new CreateTheme
    {
      PathId = path.Id,
      ModuleId = module.Id,
      Title = "New Theme",
      Description = "New Theme Description",
      Necessity = Necessity.Other,
      Complexity = Complexity.Beginner,
      Order = 1
    };

    var createdTheme = await SendAsync(command);

    var theme = await FindAsync<Theme>(createdTheme.Id);

    Assert.That(theme, Is.Not.Null);
    Assert.That(theme.Title, Is.EqualTo(command.Title));
    Assert.That(theme.Description, Is.EqualTo(command.Description));
    Assert.That(theme.ModuleId, Is.EqualTo(module.Id));
    Assert.That(theme.Necessity, Is.EqualTo(command.Necessity));
    Assert.That(theme.CreatedBy, Is.EqualTo(userId));
    Assert.That(theme.Created, Is.Not.Null);
    Assert.That(theme.Created, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }

  [Test]
  public async Task ShouldCreateThemeWithExistingSection()
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
      Title = "Module Title",
      Key = "module-key",
      Description = "Module Description"
    });

    var sect = await AddAsync(new Section
    {
      ModuleId = module.Id,
      Title = "First Section"
    });

    var command = new CreateTheme
    {
      PathId = path.Id,
      ModuleId = module.Id,
      Title = "New Theme",
      Description = "New Theme Description",
      Necessity = Necessity.Other,
      Complexity = Complexity.Beginner,
      Order = 1,
      SectionId = sect.Id
    };

    var createdTheme = await SendAsync(command);

    var theme = await FindAsync<Theme>(createdTheme.Id);

    Assert.That(theme, Is.Not.Null);
    Assert.That(theme.Title, Is.EqualTo(command.Title));
    Assert.That(theme.Description, Is.EqualTo(command.Description));
    Assert.That(theme.ModuleId, Is.EqualTo(module.Id));
    Assert.That(theme.Necessity, Is.EqualTo(command.Necessity));
    //Assert.That(theme.CreatedBy, Is.EqualTo(userId));
    Assert.That(theme.Created, Is.Not.Null);
    Assert.That(theme.Created, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }

  [Test]
  public async Task ShouldReturnNotFoundForNonExistingSection()
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
      Title = "Module Title",
      Key = "module-key",
      Description = "Module Description"
    });

    var command = new CreateTheme
    {
      ModuleId = module.Id,
      Title = "New Theme",
      Description = "New Theme Description",
      Necessity = Necessity.Other,
      Complexity = Complexity.Beginner,
      Order = 1,
      SectionId = 999
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }
}