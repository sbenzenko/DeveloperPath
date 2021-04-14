using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;

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
