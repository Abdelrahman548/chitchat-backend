namespace ChitChat.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailWithHtml(string recieverName, string recieverEmail, string subject, string body);
        Task SendEmailWithPlain(string recieverName, string recieverEmail, string subject, string body);
    }
}
