using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Domain.Entities
{
  public record Source : AuditableEntity
  {
    /// <summary>
    /// Source ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Source Title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Souce URL
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// Theme that the source is for
    /// </summary>
    public Theme Theme { get; init; }

    /// <summary>
    /// Position of source in theme (0-based).
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// Type of source (book, video, blog, etc.)
    /// </summary>
    public SourceType Type { get; init; }

    /// <summary>
    /// Whether the resource is available free or paid
    /// </summary>
    public AvailabilityLevel Availability { get; init; }
    
    /// <summary>
    /// Whether inforation if up-to-date
    /// </summary>
    public RelevanceLevel Relevance { get; init; }

    /// <summary>
    /// List of tags related to theme
    /// </summary>
    public IEnumerable<string> Tags { get; init; }
  }
}
