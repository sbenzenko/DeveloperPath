using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Common.Models
{
  public class SectionDto : IMapFrom<Section>
  {
    /// <summary>
    /// Section ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Section Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; set; }

    /// <summary>
    /// Position of section in module (0-based). 
    /// Multiple sections can have same order number (meaning they can be studied simultaneously)
    /// </summary>
    public int Order { get; set; }
  }
}