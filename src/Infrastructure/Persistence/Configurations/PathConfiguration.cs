using System;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
    public class PathConfiguration : IEntityTypeConfiguration<Path>
    {
        public void Configure(EntityTypeBuilder<Path> builder)
        {
            builder.Property(p => p.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasMaxLength(3000)
                .IsRequired();
            builder
                .Property(p => p.Tags)
                .HasConversion(
                  p => string.Join(',', p),
                  p => p.Split(',', StringSplitOptions.RemoveEmptyEntries));

            builder.HasIndex(x => x.Key).IsUnique();
            builder.Property(x => x.Key).IsRequired().HasMaxLength(100);

            // Map many-to-many Paths-to-Modules via additional entity with Order property
            builder
                .HasMany(p => p.Modules)
                .WithMany(p => p.Paths)
                .UsingEntity<PathModules>(
          j => j
              .HasOne(pm => pm.Module)
              .WithMany()
              .HasForeignKey(pm => pm.ModuleId),
          j => j
              .HasOne(pm => pm.Path)
              .WithMany()
              .HasForeignKey(pm => pm.PathId),
          j =>
          {
              j.Property(pm => pm.Order)
               .HasDefaultValue(0);
              j.HasKey(m => new { m.PathId, m.ModuleId });
          });
        }
    }
}
