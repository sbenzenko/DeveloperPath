namespace EmailSender.Interfaces
{
  public class EmailSender : IEmailSender
  {
    public string Name { get; set; }
    public string Email { get; set; }
  }
}