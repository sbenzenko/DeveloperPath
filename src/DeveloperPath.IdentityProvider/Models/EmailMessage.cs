using System;

namespace IdentityProvider.Models
{
    public class EmailMessage 
    {
        public EmailMessage(string htmlContent, string subject, string recipient)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                throw new ArgumentException($"{nameof(htmlContent)} must have a value");
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException($"{nameof(subject)} must have a value");
            if (string.IsNullOrWhiteSpace(recipient))
                throw new ArgumentException($"{nameof(recipient)} must have a value");

            HtmlContent = htmlContent;
            Subject = subject;
            Recipient = recipient;
        }

        public string HtmlContent { get;  }
        public string Subject { get; }
        public string Recipient { get;  }
    }
}