﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.CreateModule;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Shared.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Modules.Commands
{
    using static Testing;

    public class CreateModuleTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateModule();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public void ShouldReturnNotFoundForNonExistingPath()
        {
            var command = new CreateModule
            {
                Title = "New Title",
                Key = "module-key",
                Description = "New Description",
                Necessity = 0
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldRequireTitle()
        {
            var command = new CreateModule
            {
                Title = "",
                Description = "Module Decription"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Title"))
                  .Result.And
                  .Errors["Title"].Should().Contain("Title is required.");
        }

        [Test]
        public void ShouldDisallowLongTitle()
        {
            var command = new CreateModule
            {
                Title = "This module title is too long and exceeds one hundred characters allowed for module titles by CreateModuleCommandValidator",
                Description = "Learn how to design modern web applications using ASP.NET"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Title"))
                  .Result.And.Errors["Title"].Should().Contain("Title must not exceed 100 characters.");
        }

        [Test]
        public void ShouldRequireDescription()
        {
            var command = new CreateModule
            {
                Title = "Module Title"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>()
                  .Where(ex => ex.Errors.ContainsKey("Description"))
                  .Result.And.Errors["Description"].Should().Contain("Description is required.");
        }

        //[Test]
        //public async Task ShouldRequireUniqueTitle()
        //{
        //    var path = await AddAsync(new Path
        //    {
        //        Title = "Some Path",
        //        Key = "some-path",
        //        Description = "Some Path Description"
        //    });

        //    await SendAsync(new CreateModule
        //    {
        //        Key = "module-key",
        //        Title = "Module Title",
        //        Description = "Module Decription"
        //    });

        //    var command = new CreateModule
        //    {
        //        Key = "module-key-two",
        //        Title = "Module Title",
        //        Description = "Module Decription"
        //    };

        //    FluentActions.Invoking(() =>
        //        SendAsync(command)).Should().ThrowAsync<ValidationException>()
        //          .Where(ex => ex.Errors.ContainsKey("Title"))
        //          .Result.And.Errors["Title"].Should().Contain("The specified module already exists in this path.");
        //}

        [Test]
        public async Task ShouldCreateModule()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateModule
            {
                Title = "New Module",
                Key = "new-module",
                Description = "New Module Description",
                Necessity = Necessity.Other,
                Tags = new List<string> { "Tag1", "Tag2", "Tag3" }
            };

            var createdModule = await SendAsync(command);

            var module = await FindAsync<Module>(createdModule.Id);

            module.Should().NotBeNull();
            module.Title.Should().Be(command.Title);
            module.Description.Should().Be(command.Description);
            module.Necessity.Should().Be(command.Necessity);
            module.CreatedBy.Should().Be(userId);
            module.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(3000));
        }
    }
}
