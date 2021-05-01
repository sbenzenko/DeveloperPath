using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.CreateSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
  using static Testing;

  public class CreateSourceTests : TestBase
  {
    [Test]
    public void ShouldRequireMinimumFields()
    {
      var command = new CreateSource();

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>();
    }

    [Test]
    public void ShouldRequireModuleId()
    {
      var command = new CreateSource
      {
        ThemeId = 1,
        Title = "Source Title",
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("ModuleId"))
            .And.Errors["ModuleId"].Should().Contain("Module Id is required.");
    }

    [Test]
    public void ShouldRequireThemeId()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        Title = "Source Title",
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("ThemeId"))
            .And.Errors["ThemeId"].Should().Contain("Theme Id is required.");
    }

    [Test]
    public void ShouldReturnNotFoundForNonExistingModule()
    {
      var command = new CreateSource
      {
        ModuleId = 999,
        ThemeId = 1,
        Title = "Source Title",
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldReturnNotFoundForNonExistingTheme()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        ThemeId = 999,
        Title = "Source Title",
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<NotFoundException>();
    }

    [Test]
    public void ShouldRequireTitle()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        ThemeId = 1,
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title is required.");
    }

    [Test]
    public void ShouldDisallowLongTitle()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        ThemeId = 1,
        Title = "This source title is too long and exceeds two hundred characters allowed for theme titles by CreateSourceCommandValidator. And this source title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
        Description = "Source Decription",
        Url = "http://www.ww.ww"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("Title must not exceed 200 characters.");
    }

    [Test]
    public void ShouldRequireUrl()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        ThemeId = 1,
        Title = "Source Title",
        Description = "Source Decription"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Url"))
            .And.Errors["Url"].Should().Contain("URL is required.");
    }

    [Test]
    public void ShouldCheckUrlFormat()
    {
      var command = new CreateSource
      {
        ModuleId = 1,
        ThemeId = 1,
        Title = "Source Title",
        Description = "Source Decription",
        Url = "http:someinvalidurl"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Url"))
            .And.Errors["Url"].Should().Contain("URL must be in valid format, e.g. http://www.domain.com.");
    }

    [Test]
    public async Task ShouldCreateSource()
    {
      //var userId = await RunAsDefaultUserAsync();

      var path = await AddAsync(new Path
      {
        Title = "Some Path",
        Description = "Some Path Description"
      });

      var module = await SendAsync(new CreateModule
      {
        PathId = path.Id,
        Title = "Module Title",
        Description = "Module Decription"
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

      source.Should().NotBeNull();
      source.ThemeId.Should().Be(command.ThemeId);
      source.Title.Should().Be(command.Title);
      source.Description.Should().Be(command.Description);
      source.Url.Should().Be(command.Url);
      source.Type.Should().Be(command.Type);
      source.Availability.Should().Be(command.Availability);
      source.Relevance.Should().Be(command.Relevance);
     // source.CreatedBy.Should().Be(userId);
      source.Created.Should().BeCloseTo(DateTime.Now, 10000);
    }
  }
}
