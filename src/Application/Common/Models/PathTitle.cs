using DeveloperPath.Application.Common.Mappings;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Path title
  /// </summary>
  public class PathTitle : IMapFrom<Domain.Entities.Path>
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
