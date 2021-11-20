using System.Threading.Tasks;

namespace IdentityProvider.Services
{
    public interface IServiceBusSenderService
    {
        Task SendMessageToEmailQueueAsync(string callbackUrl, string userEmail);
    }
}