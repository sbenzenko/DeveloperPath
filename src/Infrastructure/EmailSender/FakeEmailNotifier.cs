using System.Threading.Tasks;
using EmailSender.Interfaces;

namespace DeveloperPath.Infrastructure.EmailSender
{
  public class FakeEmailNotifier : IEmailNotifier
  {
    public Task SendEmailAsync(IEmail email)
    {
      return Task.CompletedTask;
    }
  }
}
