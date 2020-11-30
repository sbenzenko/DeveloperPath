using System;
using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
  public class SectionConfiguration : IEntityTypeConfiguration<Section>
  {
    public void Configure(EntityTypeBuilder<Section> builder)
    {
      builder.Property(u => u.Title)
          .HasMaxLength(100)
          .IsRequired();
      builder
          .Property(u => u.Tags)
          .HasConversion(
            u => string.Join(',', u),
            u => u.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
  }
}
