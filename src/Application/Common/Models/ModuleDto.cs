using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Represents module (skill) of the path, e.g. Programming language, Databases, CI/CD. etc.
  /// </summary>
  public class ModuleDto : IMapFrom<Module>
  {
    /// <summary>
    /// Module ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Module title
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Module short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Paths module attached to
    /// </summary>
    public ICollection<PathTitle> Paths { get; init; }

    /// <summary>
    /// Necessity level (Other (default) | Possibilities | Interesting | Good to know | Must know)
    /// </summary>
    public NecessityLevel Necessity { get; init; }

    /// <summary>
    /// Modules required to know before studying this module
    /// </summary>
    public ICollection<ModuleTitle> Prerequisites { get; init; }

    /// <summary>
    /// List of tags related to the module
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
