using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.TodoLists.Commands
{
  using static Testing;

  public class CreatePathTests : TestBase
  {
    [Test]
    public void ShouldRequireMinimumFields()
    {
      var command = new CreatePathCommand();

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>();
    }

    [Test]
    public void ShouldRequireDescription()
    {
      var command = new CreatePathCommand
      {
        Title = "ASP.NET Developer"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Description"))
            .And.Errors["Description"].Should().Contain("Description is required.");
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
      await SendAsync(new CreatePathCommand
      {
        Title = "ASP.NET Developer",
        Description = "Learn how to design modern web applications using ASP.NET",
        Tags =
          new List<string>() {
            "Web", "Development", "Programming"
          }
      });

      var command = new CreatePathCommand
      {
        Title = "ASP.NET Developer",
        Description = "Learn how to design modern web applications using ASP.NET"
      };

      FluentActions.Invoking(() =>
          SendAsync(command)).Should().Throw<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey("Title"))
            .And.Errors["Title"].Should().Contain("The specified path already exists.");
    }

    [Test]
    public async Task ShouldCreatePath()
    {
      var userId = await RunAsDefaultUserAsync();

      var command = new CreatePathCommand
      {
        Title = "Some title",
        Description = "Some description"
      };

      var id = await SendAsync(command);

      var path = await FindAsync<Path>(id);

      path.Should().NotBeNull();
      path.Title.Should().Be(command.Title);
      path.Description.Should().Be(command.Description);
      path.CreatedBy.Should().Be(userId);
      path.Created.Should().BeCloseTo(DateTime.Now, 10000);
    }
  }
}
