using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IdentityProvider.Models;

namespace IdentityProvider.Services
{
    public class ServiceBusSenderService : IServiceBusSenderService
    {
        private const string VerificationEmailQueue = "email-verification-queue";

      
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusSenderService(string connectionString)
        {
            _serviceBusClient = new ServiceBusClient(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));
        }
 
        public async Task SendMessageToEmailQueueAsync(string callbackUrl, string userEmail)
        {
            var message = "<p><strong><span style=\"color: #00ccff;\">Developer Path email confirmation</span></strong></p>\r\n" +
                          "<p><span style=\"color: #000000;\"><strong>You have been registered on the Developer Path platform</strong></span></p>\r\n" +
                          $"<p><span style=\"color: #000000;\"><strong>Please, confirm your email by <a href=\"{callbackUrl}\"><span style=\"text-decoration: underline;\"><span style=\"color: #00ccff; text-decoration: underline;\">clicking here</span></span></a><span style=\"color: #00ccff;\">!</span></strong></span></p>\r\n" +
                          $"<p>&nbsp;</p>\r\n<p style=\"text-align: center;\"><strong><span style=\"background-color: #ffffff;\">" +
                          $"<code><span style=\"color: #000000;\"><span style=\"color: #00ccff;\">{{We are glad to see you aboard!}}</span></span></code></span></strong>" +
                          $"</p>\r\n<p style=\"text-align: center;\"><span style=\"background-color: #ffffff;\"><code><span style=\"color: #00ccff;\">" +
                          $"<strong>{{Keep learning}}</strong></span></code></span></p>\r\n<hr />\r\n" +
                          $"<p><span style=\"color: #808080;\">If you didn't perform any actions on the <a href=\"https://www.developer-path.com/\"><span style=\"text-decoration: underline;\">Developer Path</span></a> platform please ignore this message. </span></p>\r\n<p>&nbsp;</p>";

            var email = new EmailMessage(message, "Developer Path - Email Confirmation", userEmail);
            var sender = _serviceBusClient.CreateSender(VerificationEmailQueue);
            await sender.SendMessageAsync(new ServiceBusMessage()
            {
                Body = new BinaryData(JsonSerializer.SerializeToUtf8Bytes(email)),
                ContentType = "application/json"
            });

            await sender.CloseAsync();
        }
    }
}
