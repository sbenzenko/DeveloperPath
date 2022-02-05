using System;

namespace DeveloperPath.Domain.Entities;

public record PathModules
{
  /// <summary>
  /// Path ID
  /// </summary>
  public Guid PathId { get; init; }
  public Path Path { get; init; }

  /// <summary>
  /// Module ID
  /// </summary>
  public Guid ModuleId { get; init; }
  public Module Module { get; init; }

  /// <summary>
  /// Position of module in path (0-based). 
  /// Multiple modules can have same order number (meaning they can be studied simultaneously)
  /// </summary>
  public int Order { get; set; }
}
