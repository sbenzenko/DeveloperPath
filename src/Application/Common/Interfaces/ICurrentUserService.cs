namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Current API user interface
  /// </summary>
  public interface ICurrentUserService
  {
    /// <summary>
    /// User id
    /// </summary>
    string UserId { get; }
  }
}
