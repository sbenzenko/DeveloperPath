using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperPath.Infrastructure.Persistence.Configurations
{
  public class TagConfiguration : IEntityTypeConfiguration<Tag>
  {
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
      builder.Property(s => s.Name)
          .HasMaxLength(50)
          .IsRequired();
    }
  }
}
