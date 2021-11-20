using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeveloperPath.Application.IntegrationTests.Modules.Commands
{
    using static Testing;

    public class UpdateModuleTests : TestBase
    {
        [Test]
        public void ShouldRequireValidModuleId()
        {
            var command = new UpdateModule
            {
                Id = 99,
                Title = "New Title",
                Description = "New Description",
                Necessity = 0
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

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module",
                Description = "New Module Description"
            });

            var command = new UpdateModule
            {
                Id = module.Id,
                Title = "",
                Description = "New Description",
                Necessity = 0
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
                    .Result.And.Errors["Title"].Should().Contain("Title is required.");
        }

        [Test]
        public async Task ShouldRequireDescription()
        {
            var path = await SendAsync(new CreatePath
            {
                Title = "New Path",
                Key = "some-path",
                Description = "New Path Description"
            });

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module",
                Description = "New Module Description"
            });

            var command = new UpdateModule
            {
                Id = module.Id,
                Title = "Updated Module",
                Description = "",
                Necessity = 0
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Description"))
                    .Result.And.Errors["Description"].Should().Contain("Description is required.");
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

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module",
                Description = "New Module Description"
            });

            var command = new UpdateModule
            {
                Id = module.Id,
                Title = "This module title is too long and exceeds one hundred characters allowed for module titles by UpdateModuleCommandValidator",
                Description = "New Description",
                Necessity = 0
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
                    .Result.And.Errors["Title"].Should().Contain("Title must not exceed 100 characters.");
        }
        //todo: re-work logic of uniq module in the Path 
        //[Test]
        //public async Task ShouldRequireUniqueTitle()
        //{
        //    var path = await AddAsync(new Path
        //    {
        //        Title = "New Path",
        //        Key = "some-path",
        //        Description = "New Path Description"
        //    });

        //    var module = await SendAsync(new CreateModule
        //    {
        //        Key = "module-key",
        //        Title = "New Module",
        //        Description = "New Module Description"
        //    });

        //    await SendAsync(new CreateModule
        //    {
        //        Key = "module-key-two",
        //        Title = "Other New Module",
        //        Description = "New Other Path Description"
        //    });

        //    var command = new UpdateModule
        //    {
        //        Id = module.Id,
        //        Title = "Other New Module",
        //        Description = "New Module Description"
        //    };

        //    FluentActions.Invoking(() =>
        //        SendAsync(command))
        //            .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title"))
        //            .Result.And.Errors["Title"].Should().Contain("Module with this title already exists in one of associated paths.");
        //}

        [Test]
        public async Task ShouldUpdateModule()
        {
            var userId = await RunAsDefaultUserAsync();

            var module = await SendAsync(new CreateModule
            {
                Key = "module-key",
                Title = "New Module",
                Description = "New Module Description"
            });

            var command = new UpdateModule
            {
                Id = module.Id,
                Title = "Updated title",
                Description = "Updated Description",
                Necessity = 0
            };

            await SendAsync(command);

            var updatedModule = await FindAsync<Module>(module.Id);

            updatedModule.Title.Should().Be(command.Title);
            updatedModule.Description.Should().Be(command.Description);
            updatedModule.LastModifiedBy.Should().NotBeNull();
            updatedModule.LastModifiedBy.Should().Be(userId);
            updatedModule.LastModified.Should().NotBeNull();
            updatedModule.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(1000));
        }
    }
}
