using EmailSender.Interfaces;

namespace DeveloperPath.Infrastructure.EmailSender
{
  public class Email : IEmail
  {
    public IEmailSender EmailSender { get; set; }

    public string[] Recipients { get; set; }

    public string Subject { get; set; }

    public string PlainText { get; set; }

    public string HtmlContent { get; set; }
  }
}