using System;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Commands
{
    using static Testing;

    public class UpdatePathTests : TestBase
    {
        [Test]
        public void ShouldRequireValidPathId()
        {
            var command = new UpdatePath
            {
                Id = 99,
                Title = "New Title",
                Key = "some-path",
                Description = "New Description"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }


        [Test]
        public async Task ShouldRequireTitle()
        {
            var path = await SendAsync(new CreatePath
            {
                Title = "New Path",
                Key = "some-path",
                Description = "New Path Description"
            });

            var command = new UpdatePath
            {
                Id = path.Id,
                Title = "",
                Description = "New Path Description"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Title"))
                  .Result.And.Errors["Title"].Should().Contain("Title is required.");
        }

        [Test]
        public async Task ShouldDisallowLongTitle()
        {
            var path = await SendAsync(new CreatePath
            {
                Title = "New Path",
                Key = "some-path",
                Description = "New Path Description"
            });

            var command = new UpdatePath
            {
                Id = path.Id,
                Title = "This path title is too long and exceeds one hundred characters allowed for path titles by UpdatePathCommandValidator",
                Description = "Learn how to design modern web applications using ASP.NET"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Title"))
                  .Result.And.Errors["Title"].Should().Contain("Title must not exceed 100 characters.");
        }

        [Test]
        public async Task ShouldRequireUniqueTitle()
        {
            var path = await SendAsync(new CreatePath
            {
                Title = "New Path",
                Key = "some-path",
                Description = "New Path Description"
            });

            await SendAsync(new CreatePath
            {
                Title = "Other New Path",
                Key = "some-path-other",
                Description = "New Other Path Description"
            });

            var command = new UpdatePath
            {
                Id = path.Id,
                Title = "Other New Path",
                Description = "New Path Description"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Title"))
                  .Result.And.Errors["Title"].Should().Contain("The specified path already exists.");
        }

        [Test]
        public async Task ShouldUpdatePath()
        {
            var userId = await RunAsDefaultUserAsync();

            var path = await SendAsync(new CreatePath
            {
                Title = "New Path",
                Key = "some-path",
                Description = "New Path Description"
            });

            var command = new UpdatePath
            {
                Id = path.Id,
                Key = "some-path",
                Title = "Updated Path Title",
                Description = "New Path Description"
            };

            await SendAsync(command);

            var updatedPath = await FindAsync<Path>(path.Id);

            updatedPath.Title.Should().Be(command.Title);
            updatedPath.LastModifiedBy.Should().NotBeNull();
            updatedPath.LastModifiedBy.Should().Be(userId);
            updatedPath.LastModified.Should().NotBeNull();
            updatedPath.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(1000));
        }
    }
}
