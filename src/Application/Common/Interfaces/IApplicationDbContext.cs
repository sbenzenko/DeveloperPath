using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
  //TODO: update with DeveloperPath entities
    public interface IApplicationDbContext
    {
        DbSet<Path> Paths { get; set; }
        DbSet<Module> Modules { get; set; }
        DbSet<Section> Sections { get; set; }
        DbSet<Theme> Themes { get; set; }
        DbSet<Source> Sources { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
