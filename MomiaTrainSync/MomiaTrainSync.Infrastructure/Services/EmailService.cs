using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly EmailClient _client;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
            _client = new EmailClient(_settings.ConnectionString);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new EmailMessage(
                senderAddress: _settings.SenderAddress,
                content: new EmailContent(subject)
                {
                    Html = body,
                    PlainText = StripHtml(body) // opcional
                },
                recipients: new EmailRecipients(
                    new List<EmailAddress>
                    {
                        new EmailAddress(to)
                    }
                )
            );

            EmailSendOperation op = await _client.SendAsync(
                WaitUntil.Completed,
                emailMessage
            );

            // Puedes revisar el estatus
            var status = op.Value.Status;
        }

        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}
