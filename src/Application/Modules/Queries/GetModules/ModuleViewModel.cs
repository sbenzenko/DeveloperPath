using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Modules.Queries.GetModules
{
  /// <summary>
  /// Detailed information about the module
  /// </summary>
  public class ModuleViewModel : IMapFrom<Module>
  {
    /// <summary>
    /// Module ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Module Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Module short summary
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Paths module attached to
    /// </summary>
    public ICollection<PathTitle> Paths { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; set; }

    /// <summary>
    /// Sections that module consists of (may be empty)
    /// </summary>
    public ICollection<SectionDto> Sections { get; init; }

    /// <summary>
    /// Themes that module consists of
    /// </summary>
    public ICollection<ThemeDto> Themes { get; init; }

    /// <summary>
    /// Modules required to know before studying this module
    /// </summary>
    public ICollection<ModuleTitle> Prerequisites { get; init; }

    /// <summary>
    /// List of tags related to module
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
