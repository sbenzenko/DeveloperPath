using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Models
{
  public class ModuleTitle : IMapFrom<Module>
  {
    /// <summary>
    /// Module ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Module name
    /// </summary>
    public string Title { get; init; }
  }
}
