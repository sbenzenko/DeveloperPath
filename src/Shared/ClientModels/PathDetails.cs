using System.Collections.Generic;
using Shared.ClientModels;

namespace DeveloperPath.Domain.Shared.ClientModels
{
  /// <summary>
  /// Detailed information about the path
  /// </summary>
  public class PathDetails
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
    /// Modules that path consists of
    /// </summary>
    public IList<Module> Modules { get; init; }

    /// <summary>
    /// List of tags related to path.
    /// Use generalized tags, e.g.:
    ///  - Path ASP.NET - Tags: #Development #Web
    ///  - Path Unity - Tags: #Development #Games
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
