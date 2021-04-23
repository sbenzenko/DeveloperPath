using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using Domain.Shared.Enums;

namespace DeveloperPath.Domain.Entities
{
    /// <summary>
    /// Represents a section in module (logical group of themes, like a book chapter).
    /// May or may not exist in a module
    /// </summary>
    public record Section : AuditableEntity
  {
    /// <summary>
    /// Section ID
    /// </summary>
    //public int Id { get; init; }

    /// <summary>
    /// Theme Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Module that section is part of
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// Module that section is part of
    /// </summary>
    public Module Module { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; set; }

    /// <summary>
    /// Position of section in module (0-based). 
    /// Multiple sections can have same order number (meaning they can be studied simultaneously)
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Themes of this section
    /// </summary>
    public IList<Theme> Themes { get; init; }

    /// <summary>
    /// List of tags related to section
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}