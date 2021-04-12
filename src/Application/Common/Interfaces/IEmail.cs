namespace EmailSender.Interfaces
{
  public interface IEmail
  {
    IEmailSender EmailSender { get; set; }
    string HtmlContent { get; set; }
    string PlainText { get; set; }
    string[] Recipients { get; set; }
    string Subject { get; set; }
  }
}