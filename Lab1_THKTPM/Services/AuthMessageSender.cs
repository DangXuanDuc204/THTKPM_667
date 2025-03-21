using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Lab1_THKTPM.Configuration;

namespace Lab1_THKTPM.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        // Gửi Email
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", _settings.Value.SMTPAccount));
            emailMessage.To.Add(new MailboxAddress("User", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        // Gửi SMS (hiện tại chưa triển khai)
        public async Task SendSmsAsync(string number, string message)
        {
            // TODO: Tích hợp API SMS (ví dụ: Twilio, Firebase, v.v.)
            await Task.CompletedTask;
        }
    }
}