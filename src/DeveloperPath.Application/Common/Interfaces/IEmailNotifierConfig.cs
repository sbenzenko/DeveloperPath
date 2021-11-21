namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Email notifier configuration interface
  /// </summary>
  public interface IEmailNotifierConfig
  {
    /// <summary>
    /// Notifier service user name
    /// </summary>
    string EmailUserName { get; set; }
    /// <summary>
    /// Notifier service api key
    /// </summary>
    string EmailApiKey { get; set; }
    /// <summary>
    /// Notifier service email
    /// </summary>
    string Email { get; set; }
  }
}
