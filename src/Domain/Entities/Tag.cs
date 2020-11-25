namespace DeveloperPath.Domain.Entities
{
  /// <summary>
  /// Represents a tag attached to path/module/theme
  /// </summary>
  public record Tag
  {
    /// <summary>
    /// Tag name
    /// </summary>
    public string Name { get; init; }
  }
}