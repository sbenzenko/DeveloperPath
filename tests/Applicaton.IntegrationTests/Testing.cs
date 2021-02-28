﻿using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Common;
using DeveloperPath.Infrastructure.Identity;
using DeveloperPath.Infrastructure.Persistence;
using DeveloperPath.WebApi;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperPath.Application.IntegrationTests
{
  [SetUpFixture]
  public class Testing
  {
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static string _currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", true, true)
          .AddEnvironmentVariables();

      _configuration = builder.Build();

      var startup = new Startup(_configuration);

      var services = new ServiceCollection();

      services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
          w.EnvironmentName == "Development" &&
          w.ApplicationName == "DeveloperPath.WebApi"));

      services.AddLogging();

      startup.ConfigureServices(services);

      // Replace service registration for ICurrentUserService
      // Remove existing registration
      var currentUserServiceDescriptor = services.FirstOrDefault(d =>
          d.ServiceType == typeof(ICurrentUserService));

      services.Remove(currentUserServiceDescriptor);

      // Register testing version
      services.AddTransient(provider =>
          Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

      _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

      _checkpoint = new Checkpoint
      {
        TablesToIgnore = new[] { "__EFMigrationsHistory" }
      };

      EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
      using var scope = _scopeFactory.CreateScope();

      var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

      context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
      using var scope = _scopeFactory.CreateScope();

      var mediator = scope.ServiceProvider.GetService<IMediator>();

      return await mediator.Send(request);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
      return await RunAsUserAsync("test@local", "Testing1234!");
    }

    public static async Task<string> RunAsUserAsync(string userName, string password)
    {
      using var scope = _scopeFactory.CreateScope();

      var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

      var user = new ApplicationUser { UserName = userName, Email = userName };

      var result = await userManager.CreateAsync(user, password);

      _currentUserId = user.Id;

      return _currentUserId;
    }

    public static async Task ResetState()
    {
      await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
      _currentUserId = null;
    }

    public static async Task<TEntity> FindAsync<TEntity>(int id)
        where TEntity : class
    {
      using var scope = _scopeFactory.CreateScope();

      var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

      return await context.FindAsync<TEntity>(id);
    }

    public static async Task<TEntity> AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
      using var scope = _scopeFactory.CreateScope();

      var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

      context.Add(entity);

      await context.SaveChangesAsync();
      return entity;
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
  }
}