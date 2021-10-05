using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace DeveloperPath.EmailVerificationFn
{
    public static class EmailVerificationFn
    {
        [FunctionName(nameof(EmailVerificationFn))]
        public static async Task Run([ServiceBusTrigger("email-verification-queue", 
            Connection = "ServiceBusReceiverConnectionString")] EmailMessage emailMessage,
            [SendGrid(ApiKey = "SendGridKey", From = "%EmailFrom%")] IAsyncCollector<SendGridMessage> messageCollector,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function {nameof(EmailVerificationFn)} processed message");

            var message = new SendGridMessage();
            message.AddTo(emailMessage.Recipient);
            message.AddContent("text/html", emailMessage.HtmlContent);
            message.SetSubject(emailMessage.Subject);
            await messageCollector.AddAsync(message);
        }
    }
}
