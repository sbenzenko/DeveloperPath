using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Themes.Queries.GetTheme
{
  public class ThemeTitle : IMapFrom<Theme>
  {
    /// <summary>
    /// Theme ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Theme name
    /// </summary>
    public string Title { get; init; }
  }
}
