using System.IO;
using System.Linq;

using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Infrastructure.Persistence;
using DeveloperPath.Infrastructure.Services;
using DeveloperPath.WebApi.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests.Infrastructure;

// These tests run against live DB.
// Do NOT modify data in the database.
internal class DatabaseTests
{
  private static IConfigurationRoot _configuration;

  [OneTimeSetUp]
  public void RunBeforeAnyTests()
  {
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables()
        // using user secrets from WebApi project
        .AddUserSecrets<WebApi.Program>();

    _configuration = builder.Build();
  }

  [Test]
  public void EnsureMigrationsAreUpToDate()
  {
    var services = new ServiceCollection();
    services.AddLogging();
    services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(_configuration["DeveloperPathSqlConnectionString"],
          b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddTransient<IDateTime, DateTimeService>();
    services.AddHttpContextAccessor();

    using var serviceProvider = services.BuildServiceProvider();
    using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

    // Get required services from the dbcontext
    var migrationModelDiffer = dbContext.GetService<IMigrationsModelDiffer>();
    var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();
    var modelRuntimeInitializer = dbContext.GetService<IModelRuntimeInitializer>();
    var designTimeModel = dbContext.GetService<IDesignTimeModel>();

    // Get current model
    var model = designTimeModel.Model;

    // Get the snapshot model and finalize it
    var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;
    if (snapshotModel is IMutableModel mutableModel)
    {
      // Forces post-processing on the model such that it is ready for use by the runtime
      snapshotModel = mutableModel.FinalizeModel();
    }

    if (snapshotModel is not null)
    {
      // Validates and initializes the given model with runtime dependencies
      snapshotModel = modelRuntimeInitializer.Initialize(snapshotModel);
    }

    // Compute differences
    var modelDifferences = migrationModelDiffer.GetDifferences(
            source: snapshotModel?.GetRelationalModel(),
            target: model.GetRelationalModel());

    // The differences should be empty if the migrations are up-to-date
    Assert.That(modelDifferences.Any(), Is.False);
  }
}