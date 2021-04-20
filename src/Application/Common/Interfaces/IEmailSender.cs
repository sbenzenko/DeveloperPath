namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Email sender interface
  /// </summary>
  public interface IEmailSender
  {
    /// <summary>
    /// Sender email
    /// </summary>
    string Email { get; set; }
    /// <summary>
    /// Sender name
    /// </summary>
    string Name { get; set; }
  }
}