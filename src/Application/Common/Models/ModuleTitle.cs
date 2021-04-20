using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Module title
  /// </summary>
  public class ModuleTitle : IMapFrom<Module>
  {
    /// <summary>
    /// Module ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Module title
    /// </summary>
    public string Title { get; init; }
  }
}
