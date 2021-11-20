using DeveloperPath.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Db context interface
  /// </summary>
  public interface IApplicationDbContext
  {
    /// <summary>
    /// Paths
    /// </summary>
    DbSet<Path> Paths { get; set; }
    /// <summary>
    /// Modules
    /// </summary>
    DbSet<Module> Modules { get; set; }
    /// <summary>
    /// Sections
    /// </summary>
    DbSet<Section> Sections { get; set; }
    /// <summary>
    /// Themes
    /// </summary>
    DbSet<Theme> Themes { get; set; }
    /// <summary>
    /// Sources
    /// </summary>
    DbSet<Source> Sources { get; set; }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}
