using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Domain.Entities
{
  /// <summary>
  /// Represents module (skill) of the path, e.g. Programming language, Databases, CI/CD. etc.
  /// TODO: add Order to Paths-Modules connection table
  /// </summary>
  public record Module : AuditableEntity
  {
    /// <summary>
    /// Module ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Module Title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Module short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Paths module attached to
    /// </summary>
    public IEnumerable<Path> Paths { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; init; }

    /// <summary>
    /// Units that module consists of (may be empty)
    /// </summary>
    public IEnumerable<ModuleUnit> Units { get; init; }

    /// <summary>
    /// Themes that module consists of
    /// </summary>
    public IEnumerable<Theme> Themes { get; init; }

    /// <summary>
    /// Modules required to know before studying this module
    /// </summary>
    public IEnumerable<Module> Prerequisites { get; init; }

    /// <summary>
    /// List of tags related to module
    /// </summary>
    public IEnumerable<string> Tags { get; init; }
  }
}
