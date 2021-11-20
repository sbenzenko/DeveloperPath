namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Email interface
  /// </summary>
  public interface IEmail
  {
    /// <summary>
    /// Sender
    /// </summary>
    IEmailSender EmailSender { get; set; }
    /// <summary>
    /// Html content
    /// </summary>
    string HtmlContent { get; set; }
    /// <summary>
    /// Plain text content
    /// </summary>
    string PlainText { get; set; }
    /// <summary>
    /// Recipients array
    /// </summary>
    string[] Recipients { get; set; }
    /// <summary>
    /// Email subject
    /// </summary>
    string Subject { get; set; }
  }
}