using DeveloperPath.Application.Common.Mappings;

namespace DeveloperPath.Application.Common.Models
{
  /// <summary>
  /// Module title
  /// </summary>
  public class ModuleTitle : IMapFrom<Domain.Entities.Module>
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
