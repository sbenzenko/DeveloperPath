using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.UpdateSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Sources.Commands
{
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

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
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

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
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

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireSourceTitle()
        {
            var module = await AddAsync(new Module
            {
                Title = "New Module",
                Key = "module-key",
                Description = "New Module Description",
                Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Key = "some-path",
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
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });

            var command = new UpdateSource
            {
                Id = source.Id,
                ModuleId = module.Id,
                ThemeId = theme.Id,
                Title = "",
                Url = "https://source1.com"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
                    .Result.And.Errors["Title"].Should().Contain("Title is required.");
        }

        [Test]
        public async Task ShouldDisallowLongTitle()
        {
            var module = await AddAsync(new Module
            {
                Title = "New Module",
                Key = "module-key",
                Description = "New Module Description",
                Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Key = "some-path",
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
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });

            var command = new UpdateSource
            {
                Id = source.Id,
                ModuleId = module.Id,
                ThemeId = theme.Id,
                Title = "This source title is too long and exceeds two hundred characters allowed for theme titles by CreateSourceCommandValidator. And this source title in incredibly long and ugly. I imagine no one would create a title this long but just in case",
                Url = "https://source1.com"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
                    .Result.And.Errors["Title"].Should().Contain("Title must not exceed 200 characters.");
        }

        [Test]
        public async Task ShouldRequireUrl()
        {
            var module = await AddAsync(new Module
            {
                Title = "New Module",
                Key = "module-key",
                Description = "New Module Description",
                Paths = new List<Path> { new Path
          {
            Title = "Some Path",
            Key = "some-path",
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
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });

            var command = new UpdateSource
            {
                Id = source.Id,
                ModuleId = module.Id,
                ThemeId = theme.Id,
                Title = "Source 1"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Url"))
                    .Result.And.Errors["Url"].Should().Contain("URL is required.");
        }

        [Test]
        public async Task ShouldCheckUrlFormat()
        {
            var module = await AddAsync(new Module
            {
                Title = "New Module",
                Key = "module-key",
                Description = "New Module Description",
                Paths = new List<Path> {
                    new Path
                     {
                       Title = "Some Path",
                       Key = "some-path",
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
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });

            var command = new UpdateSource
            {
                Id = source.Id,
                ModuleId = module.Id,
                ThemeId = theme.Id,
                Title = "Source 1",
                Url = "https://someinvalidurl"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Url"))
                    .Result.And.Errors["Url"].Should().Contain("URL must be in valid format, e.g. http://www.domain.com.");
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
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
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

            updatedSource.Title.Should().Be(command.Title);
            updatedSource.Description.Should().Be(command.Description);
            updatedSource.Url.Should().Be(command.Url);
            updatedSource.LastModifiedBy.Should().NotBeNull();
            updatedSource.LastModifiedBy.Should().Be(userId);
            updatedSource.LastModified.Should().NotBeNull();
            updatedSource.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(1000));
        }
    }
}
