using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Crear el mensaje MIME
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.FromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            // Cuerpo del correo (HTML)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            // Enviar con MailKit SMTP client
            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);

            // Autenticación
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
