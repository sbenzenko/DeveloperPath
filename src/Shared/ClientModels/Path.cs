using System.Collections.Generic;

namespace Shared.ClientModels
{
  /// <summary>
  /// Developer path information
  /// </summary>
  public class Path
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path title
    /// </summary>
    public string Title { get; init; }
    
    /// <summary>
    /// URI key
    /// </summary>
    public string Key { get; init; }
    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Make path visible for users
    /// </summary>
    public bool IsVisible { get; init; }

    /// <summary>
    /// List of tags related to path
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
