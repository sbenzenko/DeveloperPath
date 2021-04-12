using System.Threading.Tasks;
using EmailSender.Interfaces;

namespace EmailSender.Implementations
{
  public class FakeEmailNotifier : IEmailNotifier
  {
    public Task SendEmailAsync(IEmail email)
    {
      return Task.CompletedTask;
    }
  }
}
