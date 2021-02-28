﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Modules.Commands.CreateModule;
using DeveloperPath.Application.Sources.Commands.UpdateSource;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class UpdateSourceTests : TestBase
  {
    [Test]
    public void ShouldRequireValidSourceId()
    {
      var command = new UpdateSourceCommand
      {
        Id = 99999,
        ModuleId = 1,
        ThemeId = 1,
        Title = "New Title",
        Url = "https://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireValidModuleId()
    {
      var command = new UpdateSourceCommand
      {
        Id = 1,
        ModuleId = 99999,
        ThemeId = 1,
        Title = "New Title",
        Url = "https://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireValidThemeId()
    {
      var command = new UpdateSourceCommand
      {
        Id = 1,
        ModuleId = 1,
        ThemeId = 99999,
        Title = "New Title",
        Url = "https://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireSourceTitle()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var source = await AddAsync(new Source
      {
        ThemeId = theme.Id,
        Title = "Source 1",
        Description = "Some description",
        Url = "https://source1.com",
        Order = 0,
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var command = new UpdateSourceCommand
      {
        Id = source.Id,
        ModuleId = module.Id,
        ThemeId = theme.Id,
        Title = "",
        Url = "https://source1.com"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Title is required.");
    }

    [Test]
    public async Task ShouldDisallowLongTitle()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var source = await AddAsync(new Source
      {
        ThemeId = theme.Id,
        Title = "Source 1",
        Description = "Some description",
        Url = "https://source1.com",
        Order = 0,
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var command = new UpdateSourceCommand
      {
        Id = source.Id,
        ModuleId = module.Id,
        ThemeId = theme.Id,
        Title = "This source title is too long and exceeds two hundred characters allowed for theme titles by CreateSourceCommandValidator. And this source title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
        Url = "https://source1.com"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
              .And.Errors["Title"].Should().Contain("Title must not exceed 200 characters.");
    }

    [Test]
    public async Task ShouldRequireUrl()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var source = await AddAsync(new Source
      {
        ThemeId = theme.Id,
        Title = "Source 1",
        Description = "Some description",
        Url = "https://source1.com",
        Order = 0,
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var command = new UpdateSourceCommand
      {
        Id = source.Id,
        ModuleId = module.Id,
        ThemeId = theme.Id,
        Title = "Source 1"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Url"))
              .And.Errors["Url"].Should().Contain("URL is required.");
    }

    [Test]
    public async Task ShouldCheckUrlFormat()
    {
      var module = await AddAsync(new Module
      {
        Title = "New Module",
        Description = "New Module Description",
        Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Description = "Some Path Description"
          }
        }
      });

      var theme = await AddAsync(new Theme
      {
        Title = "New Theme",
        Description = "New Theme Description",
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" },
        ModuleId = module.Id
      });

      var source = await AddAsync(new Source
      {
        ThemeId = theme.Id,
        Title = "Source 1",
        Description = "Some description",
        Url = "https://source1.com",
        Order = 0,
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var command = new UpdateSourceCommand
      {
        Id = source.Id,
        ModuleId = module.Id,
        ThemeId = theme.Id,
        Title = "Source 1",
        Url = "https://someinvalidurl"
      };

      FluentActions.Invoking(() =>
          SendAsync(command))
              .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Url"))
              .And.Errors["Url"].Should().Contain("URL must be in valid format, e.g. http://www.domain.com.");
    }

    [Test]
    public async Task ShouldUpdateSource()
    {
      var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModuleCommand
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
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
        Type = Domain.Enums.SourceType.Documentation,
        Availability = Domain.Enums.AvailabilityLevel.Free,
        Relevance = Domain.Enums.RelevanceLevel.Relevant,
        Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
      });

      var command = new UpdateSourceCommand
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

      updatedSource.Title.Should().Be(command.Title);
      updatedSource.Description.Should().Be(command.Description);
      updatedSource.Url.Should().Be(command.Url);
      updatedSource.LastModifiedBy.Should().NotBeNull();
      updatedSource.LastModifiedBy.Should().Be(userId);
      updatedSource.LastModified.Should().NotBeNull();
      updatedSource.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
    }
  }
}
