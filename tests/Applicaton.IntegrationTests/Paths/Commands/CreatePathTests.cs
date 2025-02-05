using System;
using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Domain.Entities;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Paths.Commands;

using static Testing;

public class CreatePathTests : TestBase
{
  [Test]
  public void ShouldRequireMinimumFields()
  {
    var command = new CreatePath();

    Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireTitle()
  {
    var command = new CreatePath
    {
      Title = "",
      Key = "path",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title is required."), Is.True);
  }

  [Test]
  public void ShouldDisallowLongTitle()
  {
    var command = new CreatePath
    {
      Title = "This path title is too long and exceeds one hundred characters allowed for path titles by CreatePathCommandValidator",
      Key = "key",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("Title must not exceed 100 characters."), Is.True);
  }

  [Test]
  public void ShouldRequireDescription()
  {
    var command = new CreatePath
    {
      Title = "ASP.NET Developer",
      Key = "key"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Description"), Is.True);
    Assert.That(ex.Errors["Description"].Contains("Description is required."), Is.True);
  }

  [Test]
  public void ShouldRequireKey()
  {
    var command = new CreatePath
    {
      Title = "ASP.NET Developer",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Key"), Is.True);
    Assert.That(ex.Errors["Key"].Contains("URI key is required."), Is.True);
  }

  [Test]
  public async Task ShouldRequireUniqueTitle()
  {
    await SendAsync(new CreatePath
    {
      Title = "ASP.NET Developer",
      Key = "asp-net",
      Description = "Learn how to design modern web applications using ASP.NET",
      Tags = ["Web", "Development", "Programming"]
    });

    var command = new CreatePath
    {
      Title = "ASP.NET Developer",
      Description = "Learn how to design modern web applications using ASP.NET"
    };

    var ex = Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    Assert.That(ex.Errors.ContainsKey("Title"), Is.True);
    Assert.That(ex.Errors["Title"].Contains("The specified path already exists."), Is.True);
  }

  [Test]
  public async Task ShouldCreatePath()
  {
    //var userId = await RunAsDefaultUserAsync();

    var command = new CreatePath
    {
      Title = "Some title",
      Key = "some-key",
      Description = "Some description"
    };

    var createdPath = await SendAsync(command);

    var path = await FindAsync<Path>(createdPath.Id);

    Assert.That(path, Is.Not.Null);
    Assert.That(path.Title, Is.EqualTo(command.Title));
    Assert.That(path.Key, Is.EqualTo(command.Key));
    Assert.That(path.Description, Is.EqualTo(command.Description));
  }
}