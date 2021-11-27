using System;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace DeveloperPath.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;
        private ValueComparer<ICollection<string>> valueComparer = new ValueComparer<ICollection<string>>((c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToList());

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
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

            foreach (var entry in ChangeTracker.Entries<IAllowSoftDeletion>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.Entity.Deleted = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
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
            builder.Entity<Domain.Entities.Module>().Property(x => x.Tags)
                .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions)null), valueComparer);
            builder.Entity<Path>().Property(x => x.Tags)
              .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
              v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions)null), valueComparer);
            builder.Entity<Theme>().Property(x => x.Tags)
             .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
             v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions)null), valueComparer);
            builder.Entity<Section>().Property(x => x.Tags)
             .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
             v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions)null), valueComparer);
            builder.Entity<Source>().Property(x => x.Tags)
             .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
             v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions)null), valueComparer);

             

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var entities = builder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(IAllowSoftDeletion).Name) != null)
                .Select(e => e.ClrType);

            Expression<Func<IAllowSoftDeletion, bool>> expression = deletion => deletion.Deleted == null;

            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                builder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }

            base.OnModelCreating(builder);
        }
    }
}
