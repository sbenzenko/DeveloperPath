using System;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
  public class SourceConfiguration : IEntityTypeConfiguration<Source>
  {
    public void Configure(EntityTypeBuilder<Source> builder)
    {
      builder.Property(s => s.Title)
          .HasMaxLength(200)
          .IsRequired();
      builder.Property(t => t.Url)
          .HasMaxLength(500)
          .IsRequired();
      builder
          .Property(s => s.Tags)
          .HasConversion(
            s => string.Join(',', s),
            s => s.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
