using System.Threading.Tasks;

namespace EmailSender.Interfaces
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(Email email);
    }
}
