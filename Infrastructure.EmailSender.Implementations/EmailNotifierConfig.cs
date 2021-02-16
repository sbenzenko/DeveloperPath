using EmailSender.Interfaces;

namespace EmailSender.Implementations
{
    public class EmailNotifierConfig: IEmailNotifierConfig
    {
        public string EmailUserName { get; set; }
        public string EmailApiKey { get; set; }
        public string Email { get; set; }
    }
}
