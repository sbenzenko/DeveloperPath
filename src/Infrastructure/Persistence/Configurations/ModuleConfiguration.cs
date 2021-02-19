using System;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
  public class ModuleConfiguration : IEntityTypeConfiguration<Module>
  {
    public void Configure(EntityTypeBuilder<Module> builder)
    {
      builder.Property(m => m.Title)
        .HasMaxLength(100)
        .IsRequired();
      builder.Property(m => m.Description)
        .HasMaxLength(3000)
        .IsRequired();
      builder
          .Property(m => m.Tags)
          .HasConversion(
            m => string.Join(',', m),
            m => m.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
