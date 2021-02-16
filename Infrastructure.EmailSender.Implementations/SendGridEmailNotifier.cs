using System;
using System.Threading.Tasks;
using EmailSender.Interfaces;
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
        public async Task SendEmailAsync(Email email)
        {
            await Execute(email);
        }

        private Task Execute(Email email)
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
