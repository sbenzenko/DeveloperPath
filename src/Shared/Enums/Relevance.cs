namespace DeveloperPath.Domain.Shared.Enums
{
  /// <summary>
  /// Relevance of the resource
  /// </summary>
  public enum Relevance
  {
    /// <summary>
    /// Not applicable
    /// </summary>
    NotApplicable,

    /// <summary>
    /// Information is up-to-date
    /// </summary>
    Relevant,

    /// <summary>
    /// Some information is outdated, some up-to-date 
    /// </summary>
    MostlyRelevant,

    /// <summary>
    /// Information is outdated or relates to outdated technology
    /// </summary>
    Obsolete
  }
}
