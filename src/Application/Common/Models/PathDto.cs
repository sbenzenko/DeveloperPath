using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Summary information about the path
  /// </summary>
  public class PathDto : IMapFrom<Path>
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path name
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// List of tags related to path.
    /// Use generalized tags, e.g.:
    ///  - Path ASP.NET - Tags: #Development #Web
    ///  - Path Unity - Tags: #Development #Games
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
