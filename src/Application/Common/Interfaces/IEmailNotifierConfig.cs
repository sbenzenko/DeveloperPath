namespace DeveloperPath.Application.Common.Interfaces
{
    public interface IEmailNotifierConfig
    {
        string EmailUserName { get; set; }
        string EmailApiKey { get; set; }
        string Email { get; set; }
    }
}
