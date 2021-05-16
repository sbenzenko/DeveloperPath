using System;
using System.Collections.Generic;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
  public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
  {
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
      builder.Property(t => t.Title)
          .HasMaxLength(200)
          .IsRequired();
      builder.Property(t => t.Description)
          .HasMaxLength(3000)
          .IsRequired();
      builder
          .Property(t => t.Tags)
          .HasConversion(
            t => string.Join(',', t),
            t => t.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
