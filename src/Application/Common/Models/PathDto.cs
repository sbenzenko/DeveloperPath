using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Developer path information
  /// </summary>
  public class PathDto : IMapFrom<Path>
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// List of tags related to path
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
