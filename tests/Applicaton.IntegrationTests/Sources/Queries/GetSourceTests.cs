using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Sources.Queries.GetSources;
using DeveloperPath.Domain.Entities;
using System.Threading;
using DeveloperPath.Shared.Enums;

namespace DeveloperPath.Application.IntegrationTests.Sources.Queries
{
    using static Testing;

    public class GetSourceTests : TestBase
    {

        [Test]
        public async Task Get_ShouldReturnSourcesList()
        {
            var path = await AddAsync(
              new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Other Module",
                Description = "New Other Module Description",
                Necessity = Necessity.MustKnow,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });
            var theme = await AddAsync(new Theme
            {
                Title = "New Theme1",
                ModuleId = module.Id,
                Description = "New Theme1 Description",
                Necessity = Necessity.MustKnow,
                Complexity = Complexity.Beginner,
                Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
                Order = 1
            });
            await AddAsync(new Source
            {
                ThemeId = theme.Id,
                Title = "Source 1",
                Url = "https://source1.com",
                Order = 0,
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant
            });
            await AddAsync(new Source
            {
                ThemeId = theme.Id,
                Title = "Source 2",
                Url = "https://source2.com",
                Order = 1,
                Type = SourceType.Documentation,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant
            });
            await AddAsync(new Source
            {
                ThemeId = theme.Id,
                Title = "Source 3",
                Url = "https://source3.com",
                Order = 2,
                Type = SourceType.Blog,
                Availability = Availability.Free,
                Relevance = Relevance.Relevant
            });

            var query = new GetSourceListQuery { PathId = path.Id, ModuleId = module.Id, ThemeId = theme.Id };

            var result = await SendAsync(query);

            result.Should().HaveCount(3);
            result.ToList()[1].Title.Should().Be("Source 2");
            result.ToList()[2].Url.Should().Be("https://source3.com");
        }

        [Test]
        public void Get_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetSourceListQuery() { PathId = 1, ModuleId = 1, ThemeId = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public async Task GetOne_ShouldReturnSource()
        {
            var path = await AddAsync(
              new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module Module",
                Description = "New Module Description",
                Necessity = Necessity.MustKnow
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

            var query = new GetSourceQuery() { PathId = path.Id, ModuleId = module.Id, ThemeId = theme.Id, Id = source.Id };

            var createdSource = await SendAsync(query);

            createdSource.Id.Should().Be(source.Id);
            createdSource.Title.Should().Be("Source 1");
            createdSource.Description.Should().Be("Some description");
            createdSource.Url.Should().Be("https://source1.com");
            createdSource.Type.Should().Be(SourceType.Documentation);
            createdSource.ThemeId.Should().Be(theme.Id);
            createdSource.Tags.Should().HaveCount(3);
        }

        [Test]
        public void GetOne_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetSourceQuery() { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public async Task GetDetails_ShouldReturnSourceDetails()
        {
            var path = await AddAsync(
              new Path { Title = "Some Path", Key = "some-path", Description = "Some Path Description" });

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module Module",
                Description = "New Module Description",
                Necessity = Necessity.MustKnow
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

            var query = new GetSourceDetailsQuery() { PathId = path.Id, ModuleId = module.Id, ThemeId = theme.Id, Id = source.Id };

            var createdSource = await SendAsync(query);

            createdSource.Id.Should().Be(source.Id);
            createdSource.Title.Should().Be("Source 1");
            createdSource.Description.Should().Be("Some description");
            createdSource.Url.Should().Be("https://source1.com");
            createdSource.Type.Should().Be(SourceType.Documentation);
            createdSource.ThemeId.Should().Be(theme.Id);
            createdSource.Tags.Should().HaveCount(3);
        }

        [Test]
        public void GetDetails_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetSourceDetailsQuery() { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public void ListShouldReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetSourceListQuery() { PathId = 99999, ModuleId = 1, ThemeId = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ListShouldReturnNotFound_WhenModuleIdNotFound()
        {
            var query = new GetSourceListQuery() { PathId = 1, ModuleId = 99999, ThemeId = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ListShouldReturnNotFound_WhenThemeIdNotFound()
        {
            var query = new GetSourceListQuery() { PathId = 1, ModuleId = 1, ThemeId = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenIdNotFound()
        {
            var query = new GetSourceQuery() { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenThemeIdNotFound()
        {
            var query = new GetSourceQuery() { PathId = 1, ModuleId = 1, ThemeId = 99999, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenModuleIdNotFound()
        {
            var query = new GetSourceQuery() { PathId = 1, ModuleId = 99999, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetSourceQuery() { PathId = 99999, ModuleId = 1, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsShouldReturnNotFound_WhenIdNotFound()
        {
            var query = new GetSourceDetailsQuery() { PathId = 1, ModuleId = 1, ThemeId = 1, Id = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsShouldReturnNotFound_WhenThemeIdNotFound()
        {
            var query = new GetSourceDetailsQuery() { PathId = 1, ModuleId = 1, ThemeId = 99999, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsShouldReturnNotFound_WhenModuleIdNotFound()
        {
            var query = new GetSourceDetailsQuery() { PathId = 1, ModuleId = 99999, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsShouldReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetSourceDetailsQuery() { PathId = 99999, ModuleId = 1, ThemeId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }
    }
}