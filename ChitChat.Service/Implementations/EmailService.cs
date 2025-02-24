using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace ChitChat.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions emailOptions;

        public EmailService(EmailOptions emailOptions)
        {
            this.emailOptions = emailOptions;
        }

        public async Task SendEmailWithHtml(string recieverName, string recieverEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailOptions.SenderName, emailOptions.SenderEmail));
            email.To.Add(new MailboxAddress(recieverName, recieverEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailOptions.SmtpServer, emailOptions.PortSSL, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(emailOptions.SenderEmail, emailOptions.AppPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailWithPlain(string recieverName, string recieverEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailOptions.SenderName, emailOptions.SenderEmail));
            email.To.Add(new MailboxAddress(recieverName, recieverEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailOptions.SmtpServer, emailOptions.PortSSL, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(emailOptions.SenderEmail, emailOptions.AppPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
