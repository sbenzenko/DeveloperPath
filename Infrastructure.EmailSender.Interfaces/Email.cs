namespace EmailSender.Interfaces
{
    public class Email
    {
        public EmailSender EmailSender { get; set; }

        public string[] Recipients { get; set; }

        public string Subject { get; set; }

        public string PlainText { get; set; }

        public string HtmlContent { get; set; }
    }
}