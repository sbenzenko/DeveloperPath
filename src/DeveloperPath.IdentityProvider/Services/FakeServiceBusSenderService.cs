using System.Threading.Tasks;

namespace IdentityProvider.Services;

public class FakeServiceBusSenderService(string connectionString) : IServiceBusSenderService
{
  public async Task SendMessageToEmailQueueAsync(string callbackUrl, string userEmail)
  {
    await Task.CompletedTask;
  }
}