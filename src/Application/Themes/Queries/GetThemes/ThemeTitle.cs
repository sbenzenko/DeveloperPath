using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Themes.Queries.GetThemes
{
  /// <summary>
  /// Theme title
  /// </summary>
  public class ThemeTitle : IMapFrom<Theme>
  {
    /// <summary>
    /// Theme id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Theme title
    /// </summary>
    public string Title { get; init; }
  }
}
