using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using eVote360Pro.Core.Domain.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace eVote360Pro.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.EmailFrom);
                if (!string.IsNullOrEmpty(_mailSettings.DisplayName))
                {
                    email.Sender.Name = _mailSettings.DisplayName;
                }
                email.From.Add(email.Sender);
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
                
                if (!string.IsNullOrEmpty(_mailSettings.SmtpUser) && !string.IsNullOrEmpty(_mailSettings.SmtpPass))
                {
                    await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                }

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo electrónico.", ex);
            }
        }
    }
}
