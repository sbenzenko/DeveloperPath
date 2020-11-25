using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Domain.Entities
{
  /// <summary>
  /// Represents a unit in module (logical group of themes, like a book chapter).
  /// May or may not exist in a module
  /// </summary>
  public record ModuleUnit : AuditableEntity
  {
    /// <summary>
    /// Unit ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Theme Title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Module that unit is part of
    /// </summary>
    public Module Module { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; init; }

    /// <summary>
    /// Position of unit in module (0-based). 
    /// Multiple units can have same order number (meaning they can be studied simultaneously)
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// Themes of this unit
    /// </summary>
    public IEnumerable<Theme> Themes { get; init; }

    /// <summary>
    /// List of tags related to unit
    /// </summary>
    public IEnumerable<string> Tags { get; init; }
  }
}