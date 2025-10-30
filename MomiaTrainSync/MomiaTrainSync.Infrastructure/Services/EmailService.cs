using Microsoft.Extensions.Options;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Services;
using System.Net;
using System.Net.Mail;
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
            var mail = new MailMessage()
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
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
