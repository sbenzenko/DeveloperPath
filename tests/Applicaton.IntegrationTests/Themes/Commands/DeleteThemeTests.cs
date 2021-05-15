using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Themes.Commands.DeleteTheme;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
    using static Testing;

    public class DeleteThemeTests : TestBase
    {
        [Test]
        public void ShouldRequireValidPathId()
        {
            var command = new DeleteTheme { PathId = 999999, ModuleId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
        [Test]
        public void ShouldRequireValidModuleId()
        {
            var command = new DeleteTheme { PathId = 1, ModuleId = 999999, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
        [Test]
        public void ShouldRequireValidThemeId()
        {
            var command = new DeleteTheme { PathId = 1, ModuleId = 1, Id = 999999 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTheme()
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

            var themeAdded = await FindAsync<Theme>(theme.Id);

            await SendAsync(new DeleteTheme
            {
                PathId = path.Id,
                ModuleId = module.Id,
                Id = theme.Id
            });

            var themeDeleted = await FindAsync<Theme>(theme.Id);

            themeAdded.Should().NotBeNull();
            themeDeleted.Should().BeNull();
        }
    }
}
