using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Commands.DeleteSource;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
    using static Testing;

    public class DeleteSourceTests : TestBase
    {
        [Test]
        public void ShouldRequireValidPathId()
        {
            var command = new DeleteSource { PathId = 999999, ModuleId = 1, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
        [Test]
        public void ShouldRequireValidModuleId()
        {
            var command = new DeleteSource { PathId = 1, ModuleId = 999999, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
        [Test]
        public void ShouldRequireValidThemeId()
        {
            var command = new DeleteSource { PathId = 1, ModuleId = 1, ThemeId = 999999, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldRequireValidSourceId()
        {
            var command = new DeleteSource { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 999999 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteSource()
        {
            var path = await AddAsync(new Path
            {
                Title = "Some Path",
                Key = "some-path",
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

            var sourceAdded = await FindAsync<Source>(source.Id);

            await SendAsync(new DeleteSource
            {
                PathId = path.Id,
                ModuleId = module.Id,
                ThemeId = theme.Id,
                Id = source.Id
            });

            var sourceDeleted = await FindAsync<Source>(source.Id);

            sourceAdded.Should().NotBeNull();
            sourceDeleted.Should().BeNull();
        }
    }
}
