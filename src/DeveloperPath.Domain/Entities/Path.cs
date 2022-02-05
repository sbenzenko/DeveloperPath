using System;
using System.Collections.Generic;
using DeveloperPath.Domain.Common;

namespace DeveloperPath.Domain.Entities;

/// <summary>
/// Represents developer path entity
/// </summary>
public record Path : AuditableEntity, IAllowSoftDeletion
{
  public string Key { get; set; }
  /// <summary>
  /// Path title
  /// </summary>
  public string Title { get; set; }

  /// <summary>
  /// Path short summary
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// If flag is false the Pass will be visible only in admin zone
  /// </summary>
  public bool IsVisible { get; set; }

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

  /// <summary>
  /// Date deleted
  /// </summary>
  public DateTime? Deleted { get; set; }
}
