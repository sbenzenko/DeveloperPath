using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Domain.Entities
{
  public record Theme : AuditableEntity
  {
    /// <summary>
    /// Theme ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Theme Title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Theme short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Module that theme is in
    /// </summary>
    public Module Module { get; init; }

    /// <summary>
    /// Unit that theme is in (can be null)
    /// </summary>
    public ModuleUnit? Unit { get; init; }

    /// <summary>
    /// Complexity level
    /// </summary>
    public ComplexityLevel Complexity { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; init; }

    /// <summary>
    /// Position of theme in module (0-based). 
    /// Multiple themes can have same order number (meaning they can be studied simultaneously)
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// Sources fo this theme
    /// </summary>
    public IEnumerable<Source> Sources { get; init; }

    /// <summary>
    /// Themes required to know before studying this theme
    /// </summary>
    public IEnumerable<Theme> Prerequisites { get; init; }

    /// <summary>
    /// Related themes ("See also" section)
    /// </summary>
    public IEnumerable<Theme> Related { get; init; }

    /// <summary>
    /// List of tags related to theme
    /// </summary>
    public IEnumerable<string> Tags { get; init; }
  }
}