using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Infrastructure.EmailSender;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailSender.Implementations
{
    public class SendGridEmailNotifier : IEmailNotifier
    {
        private readonly string _apiKey;

        public SendGridEmailNotifier(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Sends email letter
        /// </summary>
        /// <param name="email"><seealso cref="Email"/> letter model</param>
        /// <returns>SendGrid <seealso cref="Response"/></returns>
        public async Task SendEmailAsync(IEmail email)
        {
            await Execute(email);
        }

        private Task Execute(IEmail email)
        {
            var client = new SendGridClient(_apiKey);
            var letter = new SendGridMessage
            {
                From = new EmailAddress(email.EmailSender.Email, email.EmailSender.Name),
                Subject = email.Subject,
                PlainTextContent = email.PlainText,
                HtmlContent = email.HtmlContent ?? email.PlainText
            };

            foreach (var recipient in email.Recipients)
                letter.AddTo(new EmailAddress(recipient));
            
            return client.SendEmailAsync(letter);
        }
    }
}
