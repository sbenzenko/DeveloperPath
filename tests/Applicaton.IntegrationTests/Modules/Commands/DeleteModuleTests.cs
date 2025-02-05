using System.Linq;
using System.Threading.Tasks;

using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.CQRS.Modules.Commands.DeleteModule;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Domain.Entities;


using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Modules.Commands;

using static Testing;

public class DeleteModuleTests : TestBase
{
  [Test]
  public void ShouldRequireValidPathId()
  {
    var command = new DeleteModule { PathId = 999999, Id = 1 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public void ShouldRequireValidModuleId()
  {
    var command = new DeleteModule { PathId = 1, Id = 999999 };

    Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
  }

  [Test]
  public async Task ShouldRemoveModuleFromPath()
  {
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      Paths = [
          new Path
          {
            Title = "Some Path1",
            Key = "some-path1",
            Description = "Some Path Description"
          },
          new Path
          {
            Title = "Some Path2",
            Key = "some-path2",
            Description = "Some Path Description"
          }
      ]
    });

    var query = new GetModuleQuery() { PathKey = "some-path1", Id = module.Id };
    var moduleAdded = await SendAsync(query);

    await SendAsync(new DeleteModule
    {
      PathId = module.Paths.FirstOrDefault().Id,
      Id = module.Id
    });

    query = new GetModuleQuery() { PathKey = "some-path1", Id = module.Id };
    var moduleRemovedFromPath1 = await SendAsync(query);

    Assert.That(moduleAdded, Is.Not.Null);
    Assert.That(moduleAdded.Paths, Has.Count.EqualTo(2));
    Assert.That(moduleRemovedFromPath1, Is.Not.Null);
    Assert.That(moduleRemovedFromPath1.Paths, Has.Count.EqualTo(1));
  }

  [Test]
  public async Task ShouldDeleteModule()
  {
    var module = await AddAsync(new Module
    {
      Title = "New Module",
      Key = "module-key",
      Description = "New Module Description",
      Tags = ["Tag1", "Tag2", "Tag3"],
      Paths = [ new Path
              {
                Title = "Some Path1",
                Key = "some-path1",
                Description = "Some Path Description"
              }]
    });

    var moduleAdded = await FindAsync<Module>(module.Id);

    await SendAsync(new DeleteModule
    {
      PathId = module.Paths.FirstOrDefault().Id,
      Id = module.Id
    });

    var moduleDeleted = await FindAsync<Module>(module.Id);

    Assert.That(moduleAdded, Is.Not.Null);
    Assert.That(moduleDeleted, Is.Null);
  }
}