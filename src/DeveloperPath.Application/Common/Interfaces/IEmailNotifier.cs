using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
  /// <summary>
  /// Email notifier interface
  /// </summary>
  public interface IEmailNotifier
  {
    /// <summary>
    /// Sending email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task SendEmailAsync(IEmail email);
  }
}
