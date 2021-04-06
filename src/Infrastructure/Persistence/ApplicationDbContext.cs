﻿using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Infrastructure.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Infrastructure.Persistence
{
  public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
  {
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private IDbContextTransaction _currentTransaction;

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        ICurrentUserService currentUserService,
        IDateTime dateTime) : base(options, operationalStoreOptions)
    {
      _currentUserService = currentUserService;
      _dateTime = dateTime;
    }

    public DbSet<Path> Paths { get; set; }
    public DbSet<Domain.Entities.Module> Modules { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Source> Sources { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.Entity.CreatedBy = _currentUserService.UserId;
            entry.Entity.Created = _dateTime.Now;
            break;
          case EntityState.Modified:
            entry.Entity.LastModifiedBy = _currentUserService.UserId;
            entry.Entity.LastModified = _dateTime.Now;
            break;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        return;
      }

      _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync()
    {
      try
      {
        await SaveChangesAsync().ConfigureAwait(false);

        _currentTransaction?.Commit();
      }
      catch
      {
        RollbackTransaction();
        throw;
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }

    public void RollbackTransaction()
    {
      try
      {
        _currentTransaction?.Rollback();
      }
      finally
      {
        if (_currentTransaction != null)
        {
          _currentTransaction.Dispose();
          _currentTransaction = null;
        }
      }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(builder);
    }
  }
}
