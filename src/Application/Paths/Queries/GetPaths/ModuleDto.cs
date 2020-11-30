using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  public class ModuleDto : IMapFrom<Module>
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path name
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; init; }
  }
}
