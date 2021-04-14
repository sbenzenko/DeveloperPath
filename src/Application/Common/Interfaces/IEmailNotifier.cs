using System.Threading.Tasks;

namespace DeveloperPath.Application.Common.Interfaces
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(IEmail email);
    }
}
