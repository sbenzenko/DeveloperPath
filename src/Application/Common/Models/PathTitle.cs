using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Path title
  /// </summary>
  public class PathTitle : IMapFrom<Path>
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path title
    /// </summary>
    public string Title { get; init; }
  }
}
