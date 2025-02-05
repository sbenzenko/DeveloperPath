using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Themes.Commands.UpdateTheme;
using DeveloperPath.Domain.Entities;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Themes.Commands;

using static Testing;

public class UpdateThemeTests : TestBase
{
  [Test]
  public void ShouldRequireValidThemeId()
  {
    var command = new UpdateTheme
    {
      Id = 99999,
      ModuleId = 1,
      Title = "New Title",
      Description = "New Description",
      Necessity = 0
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireValidModuleId()
  {
    var command = new UpdateTheme
    {
      Id = 1,
      ModuleId = 99999,
      Title = "New Title",
      Description = "New Description",
      Necessity = 0
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public async Task ShouldRequireThemeTitle()
  {
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Paths = [ new Path
        {
          Title = "Some Path",
          Key = "some-path",
          Description = "Some Path Description"
        }
      ]
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      ModuleId = module.Id
    });

    var command = new UpdateTheme
    {
      Id = theme.Id,
      ModuleId = module.Id,
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
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Paths = [ new Path
        {
          Title = "Some Path",
          Key = "some-path",
          Description = "Some Path Description"
        }
      ]
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      ModuleId = module.Id
    });

    var command = new UpdateTheme
    {
      Id = theme.Id,
      ModuleId = module.Id,
      Title = "New Theme",
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
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Paths = [ new Path
        {
          Title = "Some Path",
          Key = "some-path",
          Description = "Some Path Description"
        }
      ]
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      ModuleId = module.Id
    });

    var command = new UpdateTheme
    {
      Id = theme.Id,
      Title = "This theme title is too long and exceeds two hundred characters allowed for theme titles by CreateThemeCommandValidator. And this theme title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
      Description = "New Description",
      Necessity = 0
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 200 characters."), Is.True);
  }

  [Test]
  public async Task ShouldRequireUniqueTitle()
  {
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Paths = [ new Path
        {
          Title = "Some Path",
          Key = "some-path",
          Description = "Some Path Description"
        }
      ]
    });

    await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      ModuleId = module.Id
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Other Theme",
      Description = "New Other Theme Description",
      ModuleId = module.Id
    });

    var command = new UpdateTheme
    {
      Id = theme.Id,
      ModuleId = module.Id,
      Title = "New Theme",
      Description = "Updated Description"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("A theme with this title already exists in this module."), Is.True);
  }

  [Test]
  public async Task ShouldUpdateTheme()
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
      Key = "module-key",
      Title = "Module Title",
      Description = "Module Description"
    });

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      ModuleId = module.Id
    });

    var command = new UpdateTheme
    {
      PathId = path.Id,
      ModuleId = module.Id,
      Id = theme.Id,
      Title = "Updated title",
      Description = "Updated Description",
      Necessity = 0
    };

    await SendAsync(command);

    var updatedTheme = await FindAsync<Theme>(theme.Id);

    Assert.That(updatedTheme, Is.Not.Null);
    Assert.That(updatedTheme.Title, Is.EqualTo(command.Title));
    Assert.That(updatedTheme.Description, Is.EqualTo(command.Description));
    Assert.That(updatedTheme.Necessity, Is.EqualTo(command.Necessity));
    Assert.That(updatedTheme.LastModifiedBy, Is.Not.Null);
    Assert.That(updatedTheme.LastModifiedBy, Is.EqualTo(userId));
    Assert.That(updatedTheme.LastModified, Is.Not.Null);
    Assert.That(updatedTheme.LastModified, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }
}