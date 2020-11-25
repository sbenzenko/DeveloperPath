using System.Collections.Generic;
using DeveloperPath.Domain.Common;

namespace DeveloperPath.Domain.Entities
{
  /// <summary>
  /// Represents developer path entity
  /// </summary>
  public record Path : AuditableEntity
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
    public IEnumerable<Module> Modules { get; init; }

    /// <summary>
    /// List of tags related to path.
    /// Use generalized tags, e.g.:
    ///  - Path ASP.NET - Tags: #Development #Web
    ///  - Path Unity - Tags: #Development #Games
    /// </summary>
    public IEnumerable<string> Tags { get; init; }
  }
}
