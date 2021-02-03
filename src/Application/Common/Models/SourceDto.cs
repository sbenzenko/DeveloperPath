using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Common.Models
{
  public class SourceDto : IMapFrom<Source>
  {
    /// <summary>
    /// Source ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Source Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Souce Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Souce URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Theme id that the source is for
    /// </summary>
    public int ThemeId { get; set; }

    /// <summary>
    /// Theme that the source is for
    /// </summary>
    public ThemeDto Theme { get; init; }

    /// <summary>
    /// Position of source in theme (0-based).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Type of source (book, video, blog, etc.)
    /// </summary>
    public SourceType Type { get; set; }

    /// <summary>
    /// Whether the resource is available free or paid
    /// </summary>
    public AvailabilityLevel Availability { get; set; }

    /// <summary>
    /// Whether inforation if up-to-date
    /// </summary>
    public RelevanceLevel Relevance { get; set; }

    /// <summary>
    /// List of tags related to theme
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}