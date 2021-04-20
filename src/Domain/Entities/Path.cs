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
    //public int Id { get; init; }

    //TODO: add unique moniker (e.g. stripped Title) to use in URL, e.g. api/paths/ASPNET

    /// <summary>
    /// Path title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; set; }

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
