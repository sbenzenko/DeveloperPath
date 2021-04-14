using DeveloperPath.Application.Common.Interfaces;

namespace DeveloperPath.Infrastructure.EmailSender
{
    public class EmailSender : IEmailSender
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}