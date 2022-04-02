using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Modules.Queries
{
    using static Testing;

    public class GetModuleTests : TestBase
    {
        //[Test]
        //public async Task Get_ShouldReturnModuleList()
        //{
        //    var m1 = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key",
        //        Title = "New Module1",
        //        Description = "New Module1 Description"
        //    });
        //    var m2 = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key-two",
        //        Title = "New Module2",
        //        Description = "New Module2 Description"
        //    });
        //    var m3 = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key-three",
        //        Title = "New Module3",
        //        Description = "New Module3 Description"
        //    });
            

        //    var path = await AddAsync(
        //        new Path { Title = "Some Path", 
        //            Key = "some-path", 
        //            Description = "Some Path Description",
        //        });

        //    var query = new GetModuleListQuery { PathKey = path.Key };
        //    var result = await SendAsync(query);

        //    result.Should().HaveCount(3);
        //}

        //[Test]
        //public async Task GetPaged_ShouldReturnModuleList()
        //{
        //    var path = await AddAsync(
        //      new Path { Title = "Some Path2", Key = "some-path-2", Description = "Some Path Description" });

        //    _ = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key",
        //        Title = "New Module1",
        //        Description = "New Module1 Description"
        //    });
        //    _ = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key-two",
        //        Title = "New Module2",
        //        Description = "New Module2 Description"
        //    });
        //    _ = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key-three",
        //        Title = "New Module3",
        //        Description = "New Module3 Description"
        //    });

        //    var query = new GetModuleListQueryPaging { PathKey = path.Key, PageSize = 2, PageNumber = 2 };
        //    var result = await SendAsync(query);

        //    result.Item1.PageSize.Should().Be(2);
        //    result.Item1.PageNumber.Should().Be(2);
        //    result.Item2.Should().HaveCount(1);
        //    (result.Item2.ToList())[0].Title.Should().Be("New Module3");
        //}

        [Test]
        public async Task GetPagedOutOfRange_ShouldReturnEmpty()
        {
            var path = await AddAsync(
              new Path { Title = "Some Path3", Key = "some-path-3", Description = "Some Path Description" });

            _ = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module1",
                Description = "New Module1 Description"
            });
            _ = await SendAsync(new CreateModule
            {
                Key = "module-key-two",
                Title = "New Module2",
                Description = "New Module2 Description"
            });
            _ = await SendAsync(new CreateModule
            {
                Key = "module-key-three",
                Title = "New Module3",
                Description = "New Module3 Description"
            });

            var query = new GetModuleListQueryPaging { PathKey = path.Key, PageSize = 999, PageNumber = 3 };

            var result = await SendAsync(query);

            result.Item1.PageSize.Should().Be(999);
            result.Item1.PageNumber.Should().Be(3);
            result.Item2.Should().BeEmpty();
        }

        [Test]
        public void Get_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetModuleListQuery() { PathId = default };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public void GetPaged_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetModuleListQueryPaging { PathKey = default, PageNumber = 1, PageSize = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public async Task GetOne_ShouldReturnModule()
        {
            var module = await SendAsync(new CreateModule
            {
                Title = "New Module",
                Key = "module-key",
                Description = "New Module Description",
                Necessity = Necessity.MustKnow,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });

            var query = new GetModuleQuery() { PathKey = "some-path", Id = module.Id };

            var result = await SendAsync(query);

            result.Title.Should().NotBeEmpty();
            result.Description.Should().NotBeEmpty();
            result.Tags.Should().HaveCount(3);
        }

        [Test]
        public void GetOne_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetModuleQuery() { PathKey = "some-path", Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public async Task GetDetails_ShouldReturnModuleDetails()
        {
            var path = await AddAsync(
              new Path { Title = "Some Another Path", Key = "some-path", Description = "Path Description" });

            var module = await SendAsync(new CreateModule
            {
                Title = "New Other Module",
                Key = "module-key",
                Description = "New Other Module Description",
                Necessity = Necessity.MustKnow,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            });
            await AddAsync(new Theme
            {
                Title = "New Theme1",
                ModuleId = module.Id,
                Description = "New Theme1 Description",
                Necessity = Necessity.MustKnow,
                Complexity = Complexity.Beginner,
                Tags = new List<string> { "Theme1", "ThemeTag2", "Tag3" },
                Order = 1
            });
            await AddAsync(new Theme
            {
                Title = "New Theme2",
                ModuleId = module.Id,
                Description = "New Theme2 Description",
                Necessity = Necessity.MustKnow,
                Complexity = Complexity.Beginner,
                Tags = new List<string> { "Theme2", "ThemeTag2", "Tag3" },
                Order = 2
            });

            var query = new GetModuleDetailsQuery() { PathId = path.Id, Id = module.Id };

            var result = await SendAsync(query);

            result.Title.Should().NotBeEmpty();
            result.Description.Should().NotBeEmpty();
            result.Themes.Should().HaveCount(2);
            result.Tags.Should().HaveCount(3);
        }

        [Test]
        public void GetDetails_ShouldThrow_WhenCanceled()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var query = new GetModuleDetailsQuery() { PathId = 1, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query, cts.Token)).Should().ThrowAsync<TaskCanceledException>();
        }

        [Test]
        public void ListShouldReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetModuleListQuery() { PathId = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenModuleIdNotFound()
        {
            var query = new GetModuleQuery() { PathKey = "some-path", Id = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetModuleQuery() { PathKey = "99999", Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsShouldReturnNotFound_WhenModuleIdNotFound()
        {
            var query = new GetModuleDetailsQuery() { PathId = 1, Id = 99999 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DetailsReturnNotFound_WhenPathIdNotFound()
        {
            var query = new GetModuleDetailsQuery() { PathId = 99999, Id = 1 };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }
    }
}
