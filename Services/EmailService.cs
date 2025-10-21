using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace MomiaTrainSync.Services
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress("noreply@momiatrainsync.com", _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            using (var smtp = new SmtpClient())
            {
                smtp.Host = _settings.SmtpServer;
                smtp.Port = _settings.Port;
                smtp.EnableSsl = _settings.EnableSSL;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(mail);
            }
        }
    }
}
