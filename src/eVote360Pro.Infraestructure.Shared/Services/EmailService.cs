using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using eVote360Pro.Core.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace eVote360Pro.Infraestructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;

            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                if (!_mailSettings.EnableSmtp)
                {
                    _logger.LogInformation("Correo en modo prueba para {To}. Asunto: {Subject}. Cuerpo: {Body}", to, subject, body);
                    return;
                }

                _logger.LogInformation("Intentando enviar correo a {To} con asunto: {Subject}", to, subject);
                _logger.LogInformation("SMTP Host: {Host}, Port: {Port}, User: {User}", _mailSettings.SmtpHost, _mailSettings.SmtpPort, _mailSettings.SmtpUser);

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
                _logger.LogInformation("Conectando al servidor SMTP...");
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                _logger.LogInformation("Conectado al servidor SMTP");
                
                if (!string.IsNullOrEmpty(_mailSettings.SmtpUser) && !string.IsNullOrEmpty(_mailSettings.SmtpPass))
                {
                    _logger.LogInformation("Autenticando con usuario {User}...", _mailSettings.SmtpUser);
                    await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                    _logger.LogInformation("Autenticación exitosa");
                }

                _logger.LogInformation("Enviando correo...");
                await smtp.SendAsync(email);
                _logger.LogInformation("Correo enviado exitosamente");
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo a {To}: {Message}", to, ex.Message);
                throw new Exception("Error al enviar el correo electrónico.", ex);
            }
        }

        public async Task SendVerificationCodeAsync(string to, string nombre, string codigo)
        {
            var body = $"""
                <p>Hola {nombre},</p>
                <p>Su código de verificación para continuar con el proceso de votación es:</p>
                <p><strong>{codigo}</strong></p>
                <p>Este código tendrá una vigencia de 5 minutos.</p>
                <p>Si usted no inició este proceso, ignore este mensaje.</p>
                """;
            await SendEmailAsync(to, "Código de verificación para votar", body);
        }

        public async Task SendVotingSummaryAsync(string to, string nombre, string eleccionNombre, DateTime fecha, IEnumerable<(string Puesto, string Seleccion, string Partido)> resumen)
        {
            var lineas = string.Join("", resumen.Select(r =>
                $"<p><strong>Puesto:</strong> {r.Puesto}<br/><strong>Selección:</strong> {r.Seleccion}<br/><strong>Partido:</strong> {r.Partido}</p>"));

            var body = $"""
                <p>Hola {nombre},</p>
                <p>Su proceso de votación ha sido completado correctamente.</p>
                <p><strong>Elección:</strong> {eleccionNombre}<br/><strong>Fecha:</strong> {fecha:dd/MM/yyyy}</p>
                <h4>Resumen de selección:</h4>
                {lineas}
                <p>Gracias por ejercer su derecho al voto.</p>
                """;
            await SendEmailAsync(to, "Resumen de su participación electoral", body);
        }
    }
}
