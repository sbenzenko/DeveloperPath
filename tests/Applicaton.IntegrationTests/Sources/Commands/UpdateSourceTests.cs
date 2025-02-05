using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.UpdateSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Sources.Commands;

using static Testing;

public class UpdateSourceTests : TestBase
{
  [Test]
  public void ShouldRequireValidSourceId()
  {
    var command = new UpdateSource
    {
      Id = 99999,
      ModuleId = 1,
      ThemeId = 1,
      Title = "New Title",
      Url = "https://www.ww.ww"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireValidModuleId()
  {
    var command = new UpdateSource
    {
      Id = 1,
      ModuleId = 99999,
      ThemeId = 1,
      Title = "New Title",
      Url = "https://www.ww.ww"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireValidThemeId()
  {
    var command = new UpdateSource
    {
      Id = 1,
      ModuleId = 1,
      ThemeId = 99999,
      Title = "New Title",
      Url = "https://www.ww.ww"
    };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public async Task ShouldRequireSourceTitle()
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

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var command = new UpdateSource
    {
      Id = source.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Title = "",
      Url = "https://source1.com"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
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

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var command = new UpdateSource
    {
      Id = source.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Title = "This source title is too long and exceeds two hundred characters allowed for theme titles by CreateSourceCommandValidator. And this source title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
      Url = "https://source1.com"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 200 characters."), Is.True);
  }

  [Test]
  public async Task ShouldRequireUrl()
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

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var command = new UpdateSource
    {
      Id = source.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Title = "Source 1"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Url"), Is.True);
    Assert.That(ex.Errors["Url"].Contains("URL is required."), Is.True);
  }

  [Test]
  public async Task ShouldCheckUrlFormat()
  {
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Paths = [new Path
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

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var command = new UpdateSource
    {
      Id = source.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Title = "Source 1",
      Url = "https://someinvalidurl"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Url"), Is.True);
    Assert.That(ex.Errors["Url"].Contains("URL must be in valid format, e.g. http://www.domain.com."), Is.True);
  }

  [Test]
  public async Task ShouldUpdateSource()
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

    var theme = await AddAsync(new Theme
    {
      Title = "New Theme",
      Description = "New Theme Description",
      ModuleId = module.Id
    });

    var source = await AddAsync(new Source
    {
      ThemeId = theme.Id,
      Title = "Source 1",
      Description = "Some description",
      Url = "https://source1.com",
      Order = 0,
      Type = SourceType.Documentation,
      Availability = Availability.Free,
      Relevance = Relevance.Relevant,
      Tags = ["Tag1", "Tag2", "Tag3"]
    });

    var command = new UpdateSource
    {
      PathId = path.Id,
      ModuleId = module.Id,
      ThemeId = theme.Id,
      Id = source.Id,
      Title = "Updated title",
      Description = "Updated Description",
      Url = "https://source1.updated.com"
    };

    await SendAsync(command);

    var updatedSource = await FindAsync<Source>(source.Id);

    Assert.That(updatedSource, Is.Not.Null);
    Assert.That(updatedSource.Title, Is.EqualTo(command.Title));
    Assert.That(updatedSource.Description, Is.EqualTo(command.Description));
    Assert.That(updatedSource.Url, Is.EqualTo(command.Url));
    Assert.That(updatedSource.LastModifiedBy, Is.Not.Null);
    Assert.That(updatedSource.LastModifiedBy, Is.EqualTo(userId));
    Assert.That(updatedSource.LastModified, Is.Not.Null);
    Assert.That(updatedSource.LastModified, Is.EqualTo(DateTime.Now).Within(1000).Milliseconds);
  }
}
